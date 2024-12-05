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

namespace Services.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateServiceModel createModel, CancellationToken cancellationToken)
        {
            var specializationRelatedToService = await _unitOfWork.ServiceRepository.GetAsync(createModel.SpecializationId, cancellationToken);

            var categoryRelatedToService = await _unitOfWork.ServiceRepository.GetAsync(createModel.ServiceCategoryId, cancellationToken);

            if (specializationRelatedToService == null || !specializationRelatedToService.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {createModel.SpecializationId} is not found or not active.");
            }

            if (categoryRelatedToService == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {createModel.ServiceCategoryId} is not found.");
            }

            await _unitOfWork.ServiceRepository.CreateAsync(_mapper.Map<Service>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await _unitOfWork.ServiceRepository.GetAsync(id, cancellationToken);
            if (serviceToDelete == null)
            {
                throw new NotFoundException($"Service with id: {id} is not found. Can't delete.");
            }

            await _unitOfWork.ServiceRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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

            if (serviceToUpdate.Specialization == null || !serviceToUpdate.Specialization.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateModel.SpecializationId} is not found or not active.");
            }

            if (serviceToUpdate.ServiceCategory == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {updateModel.ServiceCategoryId} is not found.");
            }

            await _unitOfWork.ServiceRepository.UpdateAsync(_mapper.Map<Service>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
