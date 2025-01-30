using AutoMapper;
using Events.Office;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.OfficeConsumers
{
    public class UpdateOfficeConsumer(IOfficeRepository officeRepository, IMapper mapper) : IConsumer<OfficeUpdated>
    {
        private readonly IOfficeRepository _officeRepository = officeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<OfficeUpdated> context)
        {
            await _officeRepository.UpdateAsync(_mapper.Map<Office>(context.Message), CancellationToken.None);
        }
    }
}