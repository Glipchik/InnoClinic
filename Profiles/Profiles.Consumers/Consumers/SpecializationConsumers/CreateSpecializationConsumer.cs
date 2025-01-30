using AutoMapper;
using Events.Office;
using Events.Specialization;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.SpecializationConsumers
{
    public class CreateSpecializationConsumer(ISpecializationRepository specializationRepository, IMapper mapper) : IConsumer<SpecializationCreated>
    {
        private readonly ISpecializationRepository _specializationRepository = specializationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationCreated> context)
        {
            await _specializationRepository.CreateAsync(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
        }
    }
}
