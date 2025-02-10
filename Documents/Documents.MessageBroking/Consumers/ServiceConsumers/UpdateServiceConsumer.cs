using AutoMapper;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Service;
using MassTransit;

namespace Documents.MessageBroking.Consumers.ServiceConsumers
{
    public class UpdateServiceConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<ServiceUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<ServiceUpdated> context)
        {
            _unitOfWork.ServiceRepository.Update(_mapper.Map<Service>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
