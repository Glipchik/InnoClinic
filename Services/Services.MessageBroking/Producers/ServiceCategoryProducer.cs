using AutoMapper;
using Events.Service;
using Events.ServiceCategory;
using MassTransit;
using Services.Domain.Entities;
using Services.MessageBroking.Producers.Abstractions;

namespace Services.MessageBroking.Producers
{
    public class ServiceCategoryProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IServiceCategoryProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishServiceCategoryCreated(ServiceCategory serviceCategory, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ServiceCategoryCreated>(serviceCategory), cancellationToken);
        }

        public async Task PublishServiceCategoryDeleted(Guid id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new ServiceCategoryDeleted() { Id = id }, cancellationToken);
        }

        public async Task PublishServiceCategoryUpdated(ServiceCategory serviceCategory, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ServiceCategoryUpdated>(serviceCategory), cancellationToken);
        }
    }
}
