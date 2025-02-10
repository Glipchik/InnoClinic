using Documents.Domain.Entities;

namespace Documents.MessageBroking.Producers.Abstractions.ResultProducers
{
    public interface IResultProducer
    {
        Task PublishResultCreated(Result result, CancellationToken cancellationToken);
        Task PublishResultUpdated(Result result, CancellationToken cancellationToken);
        Task PublishResultDeleted(Guid resultId, CancellationToken cancellationToken);
    }
}
