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
            FileModel fileModel, 
            CreateAccountModel createAccountModel,
            CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction(cancellationToken: cancellationToken);
                var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

                var doctor = _mapper.Map<Doctor>(createDoctorModel);

                doctor.AccountId = createdAccount.Id;

                await _unitOfWork.DoctorRepository.CreateAsync(doctor, cancellationToken);

                await _fileService.Upload(fileModel.FileName, fileModel.FileStream, fileModel.ContentType);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _fileService.Remove(fileModel.FileName);
                throw;
            }
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await _unitOfWork.DoctorRepository.GetAsync(id, cancellationToken);
            if (doctorToDelete == null)
            {
                throw new NotFoundException($"Doctor with id: {id} is not found. Can't delete.");
            }

            await _unitOfWork.DoctorRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<DoctorModel>(await _unitOfWork.DoctorRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DoctorModel>>(await _unitOfWork.DoctorRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken)
        {

            var doctorToUpdate = await _unitOfWork.DoctorRepository.GetAsync(updateDoctorModel.Id, cancellationToken);
            if (doctorToUpdate == null)
            {
                throw new NotFoundException($"Doctor with id: {updateDoctorModel.Id} is not found. Can't update.");
            }

            _mapper.Map(updateDoctorModel, doctorToUpdate);

            await _unitOfWork.DoctorRepository.UpdateAsync(doctorToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
