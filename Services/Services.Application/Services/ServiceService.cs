using AutoMapper;
using Services.Application.Models;
using Services.Application.Repositories.Abstractions;
using Services.Application.Services.Abstractions;
using Services.Domain.Entities;
using Services.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper, ISpecializationRepository specializationRepository, IServiceCategoryRepository serviceCategoryRepository)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _specializationRepository = specializationRepository;
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task Create(CreateServiceModel createModel, CancellationToken cancellationToken)
        {
            // Get related to service specialization
            var specializationRelatedToService = await _specializationRepository.GetAsync(createModel.SpecializationId, cancellationToken);

            // Get related to service category
            var categoryRelatedToService = await _serviceCategoryRepository.GetAsync(createModel.ServiceCategoryId, cancellationToken);

            // If specified specialization is not active or not found, can't create entity
            if (specializationRelatedToService == null || !specializationRelatedToService.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {createModel.SpecializationId} is not found or not active.");
            }

            // If specified category not found, can't create entity
            if (categoryRelatedToService == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {createModel.ServiceCategoryId} is not found.");
            }

            await _serviceRepository.CreateAsync(_mapper.Map<Service>(createModel), cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _serviceRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<ServiceModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ServiceModel>(await _serviceRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<ServiceModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ServiceModel>>(await _serviceRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateServiceModel updateModel, CancellationToken cancellationToken)
        {
            // Get related to service specialization
            var specializationRelatedToService = await _specializationRepository.GetAsync(updateModel.SpecializationId, cancellationToken);

            // Get related to service category
            var categoryRelatedToService = await _serviceCategoryRepository.GetAsync(updateModel.ServiceCategoryId, cancellationToken);

            // If specified specialization is not active or not found, can't update entity
            if (specializationRelatedToService == null || !specializationRelatedToService.IsActive)
            {
                throw new RelatedObjectNotFoundException($"Related specialization with id {updateModel.SpecializationId} is not found or not active.");
            }

            // If specified category not found, can't create entity
            if (categoryRelatedToService == null)
            {
                throw new RelatedObjectNotFoundException($"Related category with id {updateModel.ServiceCategoryId} is not found.");
            }

            await _serviceRepository.UpdateAsync(_mapper.Map<Service>(updateModel), cancellationToken);
        }
    }
}
