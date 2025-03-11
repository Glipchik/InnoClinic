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
    public class ServiceCategoryManager : IServiceCategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceCategoryProducer _serviceCategoryProducer;

        public ServiceCategoryManager(IUnitOfWork unitOfWork, IMapper mapper, IServiceCategoryProducer serviceCategoryProducer)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceCategoryProducer = serviceCategoryProducer;
        }

        public async Task Create(CreateServiceCategoryModel createModel, CancellationToken cancellationToken)
        {
            var createdServiceCategory = await _unitOfWork.ServiceCategoryRepository.CreateAsync(_mapper.Map<ServiceCategory>(createModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _serviceCategoryProducer.PublishServiceCategoryCreated(createdServiceCategory, cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var serviceCategoryToDelete = await _unitOfWork.ServiceCategoryRepository.GetAsync(id, cancellationToken);

            if (serviceCategoryToDelete == null)
            {
                throw new NotFoundException($"Service category with id: {id} is not found. Can't delete.");
            }

            if (serviceCategoryToDelete.Services.Any())
            {   
                throw new RelatedObjectFoundException($"Related active services to category with id {id} found. Can't delete category.");
            }

            await _unitOfWork.ServiceCategoryRepository.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _serviceCategoryProducer.PublishServiceCategoryDeleted(id, cancellationToken);
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
            var serviceCategoryToUpdate = await _unitOfWork.ServiceCategoryRepository.GetAsync(updateModel.Id, cancellationToken);
            if (serviceCategoryToUpdate == null)
            {
                throw new NotFoundException($"Service category with id: {updateModel.Id} is not found. Can't update.");
            }

            var updatedServiceCategory = await _unitOfWork.ServiceCategoryRepository.UpdateAsync(_mapper.Map<ServiceCategory>(updateModel), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _serviceCategoryProducer.PublishServiceCategoryUpdated(updatedServiceCategory, cancellationToken);
        }
    }
}
