using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Application.Services.Abstractions;
using Profiles.Domain.Entities;
using Profiles.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IFileService _fileService;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
            _fileService = fileService;
        }

        public async Task Create(
            CreateDoctorModel createDoctorModel,
            FileModel? fileModel, 
            CreateAccountModel createAccountModel,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
                {

                    var specializationRelatedToDoctor = await _unitOfWork.SpecializationRepository.GetAsync(createDoctorModel.SpecializationId, cancellationToken: cancellationToken);
                    if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
                    {
                        throw new RelatedObjectNotFoundException($"Related specialization with id {createDoctorModel.SpecializationId} is not found or not active.");
                    }

                    var officeRelatedToDoctor = await _unitOfWork.OfficeRepository.GetAsync(createDoctorModel.OfficeId, cancellationToken: cancellationToken);
                    if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
                    {
                        throw new RelatedObjectNotFoundException($"Related office with id {createDoctorModel.OfficeId} is not found or not active.");
                    }

                    var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

                    var doctor = _mapper.Map<Doctor>(createDoctorModel);

                    doctor.AccountId = createdAccount.Id;

                    await _unitOfWork.DoctorRepository.CreateAsync(doctor, cancellationToken);

                    if (fileModel != null)
                    {
                        await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                        doctor.Account.PhotoFileName = fileModel.FileName;
                    }

                    await transaction.CommitAsync(cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
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
            var doctorToDelete = await _unitOfWork.DoctorRepository.GetAsync(id, cancellationToken: cancellationToken);
            if (doctorToDelete == null)
            {
                throw new NotFoundException($"Doctor with id: {id} is not found. Can't delete.");
            }

            await _unitOfWork.DoctorRepository.DeleteAsync(id, cancellationToken);

            await _fileService.Remove(doctorToDelete.Account.PhotoFileName);

            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<DoctorModel>(await _unitOfWork.DoctorRepository.GetAsync(id, cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DoctorModel>>(await _unitOfWork.DoctorRepository.GetAllAsync(cancellationToken: cancellationToken));
        }

        public async Task Update(
            UpdateDoctorModel updateDoctorModel, 
            FileModel? fileModel, 
            CancellationToken cancellationToken)
        {
            var specializationRelatedToDoctor = await _unitOfWork.SpecializationRepository.GetAsync(updateDoctorModel.SpecializationId, cancellationToken: cancellationToken);
            if (specializationRelatedToDoctor == null || !specializationRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateDoctorModel.SpecializationId} is not found or not active.");
            }

            var officeRelatedToDoctor = await _unitOfWork.OfficeRepository.GetAsync(updateDoctorModel.OfficeId, cancellationToken: cancellationToken);
            if (officeRelatedToDoctor == null || !officeRelatedToDoctor.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related office with id {updateDoctorModel.OfficeId} is not found or not active.");
            }

            var doctorToUpdate = await _unitOfWork.DoctorRepository.GetAsync(updateDoctorModel.Id, isIncluded: false, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Doctor with id: {updateDoctorModel.Id} is not found. Can't update.");

            if (fileModel != null)
            {
                var accountToUpdate = await _unitOfWork.AccountRepository.GetAsync(doctorToUpdate.AccountId, cancellationToken: cancellationToken);

                if (accountToUpdate == null)
                {
                    throw new RelatedObjectNotFoundException($"Related account with id {doctorToUpdate.AccountId} is not found.");
                }

                await _fileService.Remove(accountToUpdate.PhotoFileName);
                await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                accountToUpdate.PhotoFileName = fileModel.FileName;

                await _unitOfWork.AccountRepository.UpdateAsync(accountToUpdate, updateDoctorModel.AuthorId, cancellationToken);
            }

            _mapper.Map(updateDoctorModel, doctorToUpdate);

            await _unitOfWork.DoctorRepository.UpdateAsync(doctorToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
