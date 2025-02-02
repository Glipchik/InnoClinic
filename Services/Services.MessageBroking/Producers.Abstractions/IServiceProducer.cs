using Services.Domain.Entities;

namespace Services.MessageBroking.Producers.Abstractions
{
    public interface IServiceProducer
    {
        Task PublishServiceCreated(Service service, CancellationToken cancellationToken);
        Task PublishServiceUpdated(Service service, CancellationToken cancellationToken);
    }
}
