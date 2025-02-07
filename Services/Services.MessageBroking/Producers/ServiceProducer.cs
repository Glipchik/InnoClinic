using AutoMapper;
using MassTransit;
using Services.Domain.Entities;
using Services.MessageBroking.Producers.Abstractions;
using Events.Service;

namespace Services.MessageBroking.Producers
{
    public class ServiceProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IServiceProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishServiceCreated(Service service, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ServiceCreated>(service), cancellationToken);
        }

        public async Task PublishServiceUpdated(Service service, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ServiceUpdated>(service), cancellationToken);
        }
    }
}
