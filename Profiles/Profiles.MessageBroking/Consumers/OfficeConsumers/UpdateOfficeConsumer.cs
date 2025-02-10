using AutoMapper;
using Events.Office;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.OfficeConsumers
{
    public class UpdateOfficeConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<OfficeUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<OfficeUpdated> context)
        {
            await _unitOfWork.OfficeRepository.UpdateAsync(_mapper.Map<Office>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}