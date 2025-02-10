using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Service;
using MassTransit;

namespace Appointments.Consumers.Consumers.ServiceConsumers
{
    public class CreateServiceConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<ServiceCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ServiceCreated> context)
        {
            await _unitOfWork.ServiceRepository.CreateAsync(_mapper.Map<Service>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
