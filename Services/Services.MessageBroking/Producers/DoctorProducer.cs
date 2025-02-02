using Events.Doctor;
using MassTransit;
using Services.MessageBroking.Producers.Abstractions;

namespace Services.MessageBroking.Producers
{
    public class DoctorProducer(IPublishEndpoint publishEndpoint) : IDoctorProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task PublishDoctorDeactivated(Guid doctorId, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new DoctorDeactivated() { Id = doctorId }, cancellationToken);
        }
    }
}
