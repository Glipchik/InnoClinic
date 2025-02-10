using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Producers.Abstractions
{
    public interface IDoctorProducer
    {
        Task PublishDoctorCreated(Doctor doctor, CancellationToken cancellationToken);
        Task PublishDoctorUpdated(Doctor doctor, CancellationToken cancellationToken);
    }
}
