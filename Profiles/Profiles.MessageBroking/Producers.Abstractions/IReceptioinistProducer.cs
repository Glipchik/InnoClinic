using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Producers.Abstractions
{
    public interface IReceptioinistProducer
    {
        Task PublishReceptionistCreated(Receptionist receptionist, CancellationToken cancellationToken);
        Task PublishReceptionistUpdated(Receptionist receptionist, CancellationToken cancellationToken);
        Task PublishReceptionistDeleted(Guid id, CancellationToken cancellationToken);
    }
}
