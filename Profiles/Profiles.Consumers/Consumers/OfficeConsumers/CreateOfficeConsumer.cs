using AutoMapper;
using Events.Office;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.OfficeConsumers
{
    public class CreateOfficeConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<OfficeCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<OfficeCreated> context)
        {
            await _unitOfWork.OfficeRepository.CreateAsync(_mapper.Map<Office>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
