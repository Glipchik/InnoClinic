using AutoMapper;
using Services.Application.Models;
using Services.Application.Models.Enums;
using Services.Domain.Repositories.Abstractions;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Application.Exceptions;

namespace Services.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecializationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateSpecializationModel createModel, CancellationToken cancellationToken)
        {
            await _unitOfWork.SpecializationRepository.CreateAsync(_mapper.Map<Specialization>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            using (var transaction = _unitOfWork.BeginTransaction(cancellationToken: cancellationToken))
            {
                var specializationToDelete = await _unitOfWork.SpecializationRepository.GetAsync(id, cancellationToken);
                if (specializationToDelete == null)
                {
                    throw new NotFoundException($"Specialization with id: {id} is not found. Can't delete.");
                }

                foreach (var doctor in specializationToDelete.Doctors)
                {
                    doctor.Status = _mapper.Map<DoctorStatus>(DoctorStatusModel.Inactive);
                    await _unitOfWork.DoctorRepository.UpdateAsync(doctor, cancellationToken);
                }

                foreach (var service in specializationToDelete.Services)
                {
                    service.IsActive = false;
                    await _unitOfWork.ServiceRepository.UpdateAsync(service, cancellationToken);
                }

                await _unitOfWork.SpecializationRepository.DeleteAsync(id, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task<SpecializationModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<SpecializationModel>(await _unitOfWork.SpecializationRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<SpecializationModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<SpecializationModel>>(await _unitOfWork.SpecializationRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateSpecializationModel updateModel, CancellationToken cancellationToken)
        {
            var specializationToUpdate = await _unitOfWork.SpecializationRepository.GetAsync(updateModel.Id, cancellationToken);
            if (specializationToUpdate == null)
            {
                throw new NotFoundException($"Specialization with id: {updateModel.Id} is not found. Can't update.");
            }

            await _unitOfWork.SpecializationRepository.UpdateAsync(_mapper.Map<Specialization>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
