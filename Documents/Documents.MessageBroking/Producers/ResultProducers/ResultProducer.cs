using Documents.Domain.Entities;
using AutoMapper;
using Events.Result;
using MassTransit;
using Documents.MessageBroking.Producers.Abstractions.ResultProducers;

namespace Documents.MessageBroking.Producers.ResultProducers
{
    public class ResultProducer(IPublishEndpoint publishEndpoint, IMapper mapper) : IResultProducer
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;

        public async Task PublishResultCreated(Result result, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ResultCreated>(result), cancellationToken);
        }

        public async Task PublishResultDeleted(Guid resultId, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new ResultDeleted() { Id = resultId }, cancellationToken);
        }

        public async Task PublishResultUpdated(Result result, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(_mapper.Map<ResultUpdated>(result), cancellationToken);
        }
    }
}
