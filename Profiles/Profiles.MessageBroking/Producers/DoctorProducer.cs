using AutoMapper;
using Events.Doctor;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.MessageBroking.Producers.Abstractions;

namespace Profiles.MessageBroking.Producers
{
    public class DoctorProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IDoctorProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishDoctorCreated(Doctor doctor, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<DoctorCreated>(doctor), cancellationToken);
        }

        public async Task PublishDoctorUpdated(Doctor doctor, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<DoctorUpdated>(doctor), cancellationToken);
        }
    }
}
