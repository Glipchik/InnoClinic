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
    public class ServiceCategoryManager : IServiceCategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceCategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CreateServiceCategoryModel createModel, CancellationToken cancellationToken)
        {
            await _unitOfWork.ServiceCategoryRepository.CreateAsync(_mapper.Map<ServiceCategory>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var servicesRelatedToCategory = await _unitOfWork.ServiceRepository.GetActiveServicesByCategoryIdAsync(id, cancellationToken);
            if (servicesRelatedToCategory.Any())
            {
                throw new RelatedObjectNotFoundException($"Related active services to category with id {id} found. Can't delete category.");
            }
            await _unitOfWork.ServiceCategoryRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ServiceCategoryModel> Get(Guid id, CancellationToken cancellationToken)
        {
            return _mapper.Map<ServiceCategoryModel>(await _unitOfWork.ServiceCategoryRepository.GetAsync(id, cancellationToken));
        }

        public async Task<IEnumerable<ServiceCategoryModel>> GetAll(CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ServiceCategoryModel>>(await _unitOfWork.ServiceCategoryRepository.GetAllAsync(cancellationToken));
        }

        public async Task Update(UpdateServiceCategoryModel updateModel, CancellationToken cancellationToken)
        {
            await _unitOfWork.ServiceCategoryRepository.UpdateAsync(_mapper.Map<ServiceCategory>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
