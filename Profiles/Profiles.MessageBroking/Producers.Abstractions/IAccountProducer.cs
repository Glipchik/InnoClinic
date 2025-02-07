using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Producers.Abstractions
{
    public interface IAccountProducer
    {
        Task PublishAccountCreated(Account account, CancellationToken cancellationToken);
        Task PublishAccountDeleted(Guid id, CancellationToken cancellationToken);
    }
}
