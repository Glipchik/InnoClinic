using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Producers.Abstractions
{
    public interface IPatientProducer
    {
        Task PublishPatientCreated(Patient patient, CancellationToken cancellationToken);
        Task PublishPatientUpdated(Patient patient, CancellationToken cancellationToken);
        Task PublishPatientDeleted(Guid id, CancellationToken cancellationToken);
    }
}
