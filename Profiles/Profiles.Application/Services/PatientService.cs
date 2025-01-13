using AutoMapper;
using Profiles.Application.Exceptions;
using Profiles.Application.Models;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileService = fileService;
        }

        public async Task Create(
            CreatePatientModel createPatientModel, 
            CreateAccountModel createAccountModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction(cancellationToken: cancellationToken);
                var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

                var patient = _mapper.Map<Patient>(createPatientModel);

                patient.IsLinkedToAccount = true;
                patient.AccountId = createdAccount.Id;

                await _unitOfWork.PatientRepository.CreateAsync(patient, cancellationToken);

                if (fileModel != null)
                {
                    await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);
                    
                    patient.Account.PhotoFileName = fileModel.FileName;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                if (fileModel != null)
                {                
                    await _fileService.Remove(fileModel.FileName);
                }

                throw;
            }
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var patientToDelete = await _unitOfWork.PatientRepository.GetAsync(id, cancellationToken);
            if (patientToDelete == null)
            {
                throw new NotFoundException($"Patient with id: {id} is not found. Can't delete.");
            }

            await _unitOfWork.PatientRepository.DeleteAsync(id, cancellationToken);

            await _fileService.Remove(patientToDelete.Account.PhotoFileName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<PatientModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<PatientModel>(await _unitOfWork.PatientRepository.GetAsync(id, cancellationToken));
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
            var patientToUpdate = await _unitOfWork.PatientRepository.GetAsync(updatePatientModel.Id, cancellationToken) 
                ?? throw new NotFoundException($"Patient with id: {updatePatientModel.Id} is not found. Can't update.");

            if (fileModel != null)
            {
                await _fileService.Remove(patientToUpdate.Account.PhotoFileName);
                await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);
                
                patientToUpdate.Account.PhotoFileName = fileModel.FileName;
            }

            _mapper.Map(updatePatientModel, patientToUpdate);

            await _unitOfWork.PatientRepository.UpdateAsync(patientToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}