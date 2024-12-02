using AutoMapper;
using Services.Application.Models;
using Services.Domain.Repositories.Abstractions;
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
            // Get related to service specialization
            var specializationRelatedToService = await _unitOfWork.GetSpecializationRepository().GetAsync(createModel.SpecializationId, cancellationToken);

            // Get related to service category
            var categoryRelatedToService = await _unitOfWork.GetServiceCategoryRepository().GetAsync(createModel.ServiceCategoryId, cancellationToken);

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

            await _unitOfWork.GetServiceRepository().CreateAsync(_mapper.Map<Service>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.GetServiceRepository().DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ServiceModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ServiceModel>(await _unitOfWork.GetServiceRepository().GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<ServiceModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ServiceModel>>(await _unitOfWork.GetServiceRepository().GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateServiceModel updateModel, CancellationToken cancellationToken)
        {
            // Get related to service specialization
            var specializationRelatedToService = await _unitOfWork.GetSpecializationRepository().GetAsync(updateModel.SpecializationId, cancellationToken);

            // Get related to service category
            var categoryRelatedToService = await _unitOfWork.GetServiceCategoryRepository().GetAsync(updateModel.ServiceCategoryId, cancellationToken);

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

            await _unitOfWork.GetServiceRepository().UpdateAsync(_mapper.Map<Service>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
