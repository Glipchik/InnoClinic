using AutoMapper;
using Profiles.Application.Exceptions;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.MessageBroking.Producers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IFileService _fileService;
        private readonly IPatientProducer _patientProducer;
        private readonly IAccountProducer _accountProducer;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileService fileService,
            IPatientProducer patientProducer,
            IAccountProducer accountProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileService = fileService;
            _patientProducer = patientProducer;
            _accountProducer = accountProducer;
        }

        public async Task Create(
            CreatePatientModel createPatientModel, 
            CreateAccountModel createAccountModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {
                    var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

                    var patient = _mapper.Map<Patient>(createPatientModel);

                    patient.IsLinkedToAccount = true;
                    patient.AccountId = createdAccount.Id;

                    var createdPatient = await _unitOfWork.PatientRepository.CreateAsync(patient, cancellationToken);

                    if (fileModel != null)
                    {
                        await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);
                    
                        patient.Account.PhotoFileName = fileModel.FileName;
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    await _accountProducer.PublishAccountCreated(_mapper.Map<Account>(createdAccount), cancellationToken);
                    await _patientProducer.PublishPatientCreated(createdPatient, cancellationToken);
                }
            }
            catch
            {
                if (fileModel != null && await _fileService.DoesFileExist(fileModel.FileName))
                {                
                    await _fileService.Remove(fileModel.FileName);
                }

                throw;
            }
        }

        public async Task CreateFromAuthServer(CreatePatientModel createPatientModel, CreateAccountFromAuthServerModel createAccountFromAuthServerModel, FileModel? fileModel, CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {
                    var createdAccount = await _accountService.CreateFromAuthServer(createAccountFromAuthServerModel, cancellationToken);

                    var patient = _mapper.Map<Patient>(createPatientModel);

                    patient.IsLinkedToAccount = true;
                    patient.AccountId = createdAccount.Id;

                    await _unitOfWork.PatientRepository.CreateAsync(patient, cancellationToken);

                    if (fileModel != null)
                    {
                        await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                        patient.Account.PhotoFileName = fileModel.FileName;
                    }

                    await transaction.CommitAsync(cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _patientProducer.PublishPatientCreated(patient, cancellationToken);
                    await _accountProducer.PublishAccountCreated(_mapper.Map<Account>(createdAccount), cancellationToken);
                }
            }
            catch
            {
                if (fileModel != null && await _fileService.DoesFileExist(fileModel.FileName))
                {
                    await _fileService.Remove(fileModel.FileName);
                }

                throw;
            }
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var patientToDelete = await _unitOfWork.PatientRepository.GetAsync(id, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Patient with id: {id} is not found. Can't delete.");

            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {
                    await _unitOfWork.PatientRepository.DeleteAsync(id, cancellationToken);

                    await _unitOfWork.AccountRepository.DeleteAsync(patientToDelete.AccountId, patientToDelete.Id, cancellationToken);

                    await _fileService.Remove(patientToDelete.Account.PhotoFileName);

                    await transaction.CommitAsync(cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _patientProducer.PublishPatientDeleted(patientToDelete.Id, cancellationToken);
                    await _accountProducer.PublishAccountDeleted(patientToDelete.AccountId, cancellationToken);
                }
            }
            catch
            {
                if (await _fileService.DoesFileExist(patientToDelete.Account.PhotoFileName))
                {
                    await _fileService.Remove(patientToDelete.Account.PhotoFileName);
                }

                throw;
            }
        }

        public async Task<PatientModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<PatientModel>(await _unitOfWork.PatientRepository.GetAsync(id, cancellationToken: cancellationToken));
        }
        public async Task<PatientModel> GetByAccountId(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<PatientModel>(await _unitOfWork.PatientRepository.GetByAccountIdAsync(id, cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<PatientModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<PatientModel>>(await _unitOfWork.PatientRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(
            UpdatePatientModel updatePatientModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken)
        {
            var patientToUpdate = await _unitOfWork.PatientRepository.GetAsync(updatePatientModel.Id, isIncluded: false, cancellationToken: cancellationToken) 
                ?? throw new NotFoundException($"Patient with id: {updatePatientModel.Id} is not found. Can't update.");

            if (fileModel != null)
            {
                var accountToUpdate = await _unitOfWork.AccountRepository.GetAsync(patientToUpdate.AccountId, cancellationToken: cancellationToken);

                if (accountToUpdate == null)
                {
                    throw new RelatedObjectNotFoundException($"Related account with id {patientToUpdate.AccountId} is not found.");
                }

                await _fileService.Remove(patientToUpdate.Account.PhotoFileName);
                await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                accountToUpdate.PhotoFileName = fileModel.FileName;

                await _unitOfWork.AccountRepository.UpdateAsync(accountToUpdate, updatePatientModel.AuthorId, cancellationToken);
            }

            _mapper.Map(updatePatientModel, patientToUpdate);

            await _unitOfWork.PatientRepository.UpdateAsync(patientToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _patientProducer.PublishPatientUpdated(patientToUpdate, cancellationToken);
        }
    }
}