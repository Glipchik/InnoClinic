using Services.Domain.Entities;

namespace Services.MessageBroking.Producers.Abstractions
{
    public interface IServiceCategoryProducer
    {
        Task PublishServiceCategoryCreated(ServiceCategory serviceCategory, CancellationToken cancellationToken);
        Task PublishServiceCategoryUpdated(ServiceCategory serviceCategory, CancellationToken cancellationToken);
        Task PublishServiceCategoryDeleted(Guid id, CancellationToken cancellationToken);
    }
}
