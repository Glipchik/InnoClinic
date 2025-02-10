using AutoMapper;
using Events.Patient;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.MessageBroking.Producers.Abstractions;

namespace Profiles.MessageBroking.Producers
{
    public class PatientProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IPatientProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishPatientCreated(Patient patient, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<PatientCreated>(patient), cancellationToken);
        }

        public async Task PublishPatientDeleted(Guid id, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new PatientDeleted() { Id = id }, cancellationToken);
        }

        public async Task PublishPatientUpdated(Patient patient, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<PatientUpdated>(patient), cancellationToken);
        }
    }
}
