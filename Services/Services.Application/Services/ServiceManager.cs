using AutoMapper;
using Services.Application.Models;
using Services.Domain.Repositories.Abstractions;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.MessageBroking.Producers.Abstractions;

namespace Services.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceProducer _serviceProducer;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IServiceProducer serviceProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceProducer = serviceProducer;
        }

        public async Task Create(CreateServiceModel createModel, CancellationToken cancellationToken)
        {
            var specializationRelatedToService = await _unitOfWork.SpecializationRepository.GetAsync(createModel.SpecializationId, cancellationToken);

            var categoryRelatedToService = await _unitOfWork.ServiceCategoryRepository.GetAsync(createModel.ServiceCategoryId, cancellationToken);

            if (specializationRelatedToService == null || !specializationRelatedToService.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {createModel.SpecializationId} is not found or not active.");
            }

            if (categoryRelatedToService == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {createModel.ServiceCategoryId} is not found.");
            }

            var createdService = await _unitOfWork.ServiceRepository.CreateAsync(_mapper.Map<Service>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _serviceProducer.PublishServiceCreated(createdService, cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await _unitOfWork.ServiceRepository.GetAsync(id, cancellationToken);
            if (serviceToDelete == null)
            {
                throw new NotFoundException($"Service with id: {id} is not found. Can't delete.");
            }

            serviceToDelete.IsActive = false;

            var updatedService = await _unitOfWork.ServiceRepository.UpdateAsync(serviceToDelete, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _serviceProducer.PublishServiceUpdated(updatedService, cancellationToken);
        }

        public async Task<ServiceModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ServiceModel>(await _unitOfWork.ServiceRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<ServiceModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ServiceModel>>(await _unitOfWork.ServiceRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateServiceModel updateModel, CancellationToken cancellationToken)
        {
            var serviceToUpdate = await _unitOfWork.ServiceRepository.GetAsync(updateModel.Id, cancellationToken);
            if (serviceToUpdate == null)
            {
                throw new NotFoundException($"Service with id: {updateModel.Id} is not found. Can't update.");
            }

            var specializationRelatedToService = await _unitOfWork.SpecializationRepository.GetAsync(updateModel.SpecializationId, cancellationToken);

            var categoryRelatedToService = await _unitOfWork.ServiceCategoryRepository.GetAsync(updateModel.ServiceCategoryId, cancellationToken);

            if (specializationRelatedToService == null || !specializationRelatedToService.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateModel.SpecializationId} is not found or not active.");
            }

            if (categoryRelatedToService == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {updateModel.ServiceCategoryId} is not found.");
            }

            var updatedService = await _unitOfWork.ServiceRepository.UpdateAsync(_mapper.Map<Service>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _serviceProducer.PublishServiceUpdated(updatedService, cancellationToken);
        }
    }
}
