using AutoMapper;
using Events.Office;
using Events.Specialization;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.SpecializationConsumers
{
    public class CreateSpecializationConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<SpecializationCreated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationCreated> context)
        {
            await _unitOfWork.SpecializationRepository.CreateAsync(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
