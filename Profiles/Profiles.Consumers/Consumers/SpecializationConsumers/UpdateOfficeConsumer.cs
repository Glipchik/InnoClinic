using AutoMapper;
using Events.Specialization;
using MassTransit;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;

namespace Profiles.Consumers.Consumers.SpecializationConsumers
{
    public class UpdateSpecializationConsumer(ISpecializationRepository specializationRepository, IMapper mapper) : IConsumer<SpecializationUpdated>
    {
        private readonly ISpecializationRepository _specializationRepository = specializationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationUpdated> context)
        {
            await _specializationRepository.UpdateAsync(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
        }
    }
}