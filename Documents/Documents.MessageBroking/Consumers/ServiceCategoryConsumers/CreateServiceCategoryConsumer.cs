using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Doctor;
using Events.ServiceCategory;
using MassTransit;

namespace Documents.MessageBroking.Consumers.ServiceCategoryConsumers
{
    public class CreateServiceCategoryConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<ServiceCategoryCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ServiceCategoryCreated> context)
        {
            await _unitOfWork.ServiceCategoryRepository.CreateAsync(_mapper.Map<ServiceCategory>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
