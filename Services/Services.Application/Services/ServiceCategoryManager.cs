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
    public class ServiceCategoryManager : IServiceCategoryManager
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceCategoryManager(IServiceCategoryRepository serviceCategoryRepository, IMapper mapper, IServiceRepository serviceRepository)
        {
            _mapper = mapper;
            _serviceCategoryRepository = serviceCategoryRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task Create(CreateServiceCategoryModel createModel, CancellationToken cancellationToken)
        {
            await _serviceCategoryRepository.CreateAsync(_mapper.Map<ServiceCategory>(createModel), cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            // Get related to category services
            var servicesRelatedToCategory = await _serviceRepository.GetActiveServicesWithCategoryAsync(id, cancellationToken);
            // If specified specialization is not active or not found, can't create entity
            if (servicesRelatedToCategory.Any())
            {
                throw new RelatedObjectNotFoundException($"Related active services to category with id {id} found. Can't delete category.");
            }
        }

        public async Task<ServiceCategoryModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ServiceCategoryModel>(await _serviceCategoryRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<ServiceCategoryModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ServiceCategoryModel>>(await _serviceCategoryRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateServiceCategoryModel updateModel, CancellationToken cancellationToken)
        {
            await _serviceCategoryRepository.UpdateAsync(_mapper.Map<ServiceCategory>(updateModel), cancellationToken);
        }
    }
}
