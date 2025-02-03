using AutoMapper;
using Events.Receptionist;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.MessageBroking.Producers.Abstractions;

namespace Profiles.MessageBroking.Producers
{
    public class ReceptionistProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IReceptioinistProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishReceptionistCreated(Receptionist receptionist, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ReceptionistCreated>(receptionist), cancellationToken);
        }

        public async Task PublishReceptionistDeleted(Guid id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new ReceptionistDeleted() { Id = id }, cancellationToken);
        }

        public async Task PublishReceptionistUpdated(Receptionist receptionist, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ReceptionistUpdated>(receptionist), cancellationToken);
        }
    }
}
