using AutoMapper;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Specialization;
using MassTransit;

namespace Documents.MessageBroking.Consumers.SpecializationConsumers
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
