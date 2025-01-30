using Appointments.Domain.Entities;

namespace Appointments.MessageBroking.Producers.Abstractions.AppointmentProducers
{
    public interface IAppointmentProducer
    {
        Task PublishAppointmentCreated(Appointment appointment, CancellationToken cancellationToken);
        Task PublishAppointmentUpdated(Appointment appointment, CancellationToken cancellationToken);
        Task PublishAppointmentDeleted(Guid appointmentId, CancellationToken cancellationToken);
    }
}
