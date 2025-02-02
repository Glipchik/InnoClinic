using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Specialization;
using MassTransit;

namespace Documents.MessageBroking.Consumers.SpecializationConsumers
{
    public class UpdateSpecializationConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<SpecializationUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationUpdated> context)
        {
            _unitOfWork.SpecializationRepository.Update(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
