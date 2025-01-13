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

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task Create(CreateDoctorModel createDoctorModel, CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction(cancellationToken: cancellationToken);
            var createdAccount = await _accountService.Create(createAccountModel, cancellationToken);

            var doctor = _mapper.Map<Doctor>(createDoctorModel);

            doctor.AccountId = createdAccount.Id;

            await _unitOfWork.DoctorRepository.CreateAsync(doctor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
