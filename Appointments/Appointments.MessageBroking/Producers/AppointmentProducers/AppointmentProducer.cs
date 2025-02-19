using Appointments.Domain.Entities;
using Appointments.MessageBroking.Producers.Abstractions.AppointmentProducers;
using AutoMapper;
using Events.Appointment;
using MassTransit;

namespace Appointments.MessageBroking.Producers.AppointmentProducers
{
    public class AppointmentProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IAppointmentProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishAppointmentCreated(Appointment appointment, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<AppointmentCreated>(appointment), cancellationToken);
        }

        public async Task PublishAppointmentDeleted(Guid appointmentId, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new AppointmentDeleted() { Id = appointmentId }, cancellationToken);
        }

        public async Task PublishAppointmentUpdated(Appointment appointment, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<AppointmentUpdated>(appointment), cancellationToken);
        }
    }
}
