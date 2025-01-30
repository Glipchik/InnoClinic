using AutoMapper;
using Events.Office;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.OfficeConsumers
{
    public class CreateOfficeConsumer(IOfficeRepository officeRepository, IMapper mapper) : IConsumer<OfficeCreated>
    {
        private readonly IOfficeRepository _officeRepository = officeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<OfficeCreated> context)
        {
            await _officeRepository.CreateAsync(_mapper.Map<Office>(context.Message), CancellationToken.None);
        }
    }
}
