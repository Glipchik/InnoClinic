using AutoMapper;
using Events.Appointment;
using Events.Office;
using MassTransit;
using Offices.Data.Entities;
using Offices.MessageBroking.Producers.Abstractions;

namespace Offices.MessageBroking.Producers
{
    public class OfficeProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IOfficeProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishOfficeCreated(Office office, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<OfficeCreated>(office), cancellationToken);
        }

        public async Task PublishOfficeUpdated(Office office, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<OfficeUpdated>(office), cancellationToken);
        }
    }
}
