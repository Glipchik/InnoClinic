using Offices.Data.Entities;

namespace Offices.MessageBroking.Producers.Abstractions
{
    public interface IOfficeProducer
    {
        Task PublishOfficeCreated(Office office, CancellationToken cancellationToken);
        Task PublishOfficeUpdated(Office office, CancellationToken cancellationToken);
    }
}
