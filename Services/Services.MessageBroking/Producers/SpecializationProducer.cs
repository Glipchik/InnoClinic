using AutoMapper;
using Events.Specialization;
using MassTransit;
using Services.Domain.Entities;
using Services.MessageBroking.Producers.Abstractions;

namespace Services.MessageBroking.Producers
{
    public class SpecializationProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : ISpecializationProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishSpecializationCreated(Specialization specialization, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<SpecializationCreated>(specialization), cancellationToken);
        }

        public async Task PublishSpecializationUpdated(Specialization specialization, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<SpecializationUpdated>(specialization), cancellationToken);
        }
    }
}
