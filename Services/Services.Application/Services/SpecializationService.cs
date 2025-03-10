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
using Services.MessageBroking.Producers.Abstractions;
using Services.Domain.Models;

namespace Services.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceProducer _serviceProducer;
        private readonly IDoctorProducer _doctorProducer;
        private readonly ISpecializationProducer _specializationProducer;

        public SpecializationService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IServiceProducer serviceProducer,
            ISpecializationProducer specializationProducer,
            IDoctorProducer doctorProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceProducer = serviceProducer;
            _specializationProducer = specializationProducer;
            _doctorProducer = doctorProducer;
        }

        public async Task Create(CreateSpecializationModel createModel, CancellationToken cancellationToken)
        {
            var createdSpecialization = await _unitOfWork.SpecializationRepository.CreateAsync(_mapper.Map<Specialization>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _specializationProducer.PublishSpecializationCreated(createdSpecialization, cancellationToken);
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
                    if (doctor.Status != DoctorStatus.AtWork)
                    {
                        doctor.Status = _mapper.Map<DoctorStatus>(DoctorStatusModel.Inactive);
                        await _unitOfWork.DoctorRepository.UpdateAsync(doctor, cancellationToken);
                    }
                }

                foreach (var service in specializationToDelete.Services)
                {
                    if (service.IsActive)
                    {
                        service.IsActive = false;
                        await _unitOfWork.ServiceRepository.UpdateAsync(service, cancellationToken);
                    }
                }

                specializationToDelete.IsActive = false;
                await _unitOfWork.SpecializationRepository.UpdateAsync(specializationToDelete, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                foreach (var doctor in specializationToDelete.Doctors)
                {
                    if (doctor.Status != DoctorStatus.AtWork)
                    {
                        await _doctorProducer.PublishDoctorDeactivated(doctor.Id, cancellationToken);
                    }
                }

                foreach (var service in specializationToDelete.Services)
                {
                    if (service.IsActive)
                    {
                        await _serviceProducer.PublishServiceUpdated(service, cancellationToken);
                    }
                }

                await _specializationProducer.PublishSpecializationUpdated(specializationToDelete, cancellationToken);
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

        public async Task<PaginatedList<SpecializationModel>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var result = new List<SpecializationModel>();

            var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync(pageIndex, pageSize, cancellationToken);

            foreach (var specialization in specializations.Items)
            {
                var specializationModel = _mapper.Map<SpecializationModel>(specialization);
                result.Add(specializationModel);
            }

            return new PaginatedList<SpecializationModel>(result, specializations.PageIndex, specializations.TotalPages);
        }

        public async Task Update(UpdateSpecializationModel updateModel, CancellationToken cancellationToken)
        {
            var specializationToUpdate = await _unitOfWork.SpecializationRepository.GetAsync(updateModel.Id, cancellationToken)
                ?? throw new NotFoundException($"Specialization with id: {updateModel.Id} is not found. Can't update.");

            if (updateModel.IsActive == false && specializationToUpdate.IsActive == true)
            {
                await Delete(updateModel.Id, cancellationToken);
            }

            _mapper.Map(updateModel, specializationToUpdate);

            await _unitOfWork.SpecializationRepository.UpdateAsync(specializationToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _specializationProducer.PublishSpecializationUpdated(specializationToUpdate, cancellationToken);
        }
    }
}
