using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.ServiceCategory;
using MassTransit;

namespace Appointments.Consumers.Consumers.ServiceCategoryConsumers
{
    public class UpdateServiceCategoryConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<ServiceCategoryUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ServiceCategoryUpdated> context)
        {
            await _unitOfWork.ServiceCategoryRepository.UpdateAsync(_mapper.Map<ServiceCategory>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
