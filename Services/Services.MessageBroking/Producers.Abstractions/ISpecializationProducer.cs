using Services.Domain.Entities;

namespace Services.MessageBroking.Producers.Abstractions
{
    public interface ISpecializationProducer
    {
        Task PublishSpecializationCreated(Specialization specialization, CancellationToken cancellationToken);
        Task PublishSpecializationUpdated(Specialization specialization, CancellationToken cancellationToken);
    }
}
