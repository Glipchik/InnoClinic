using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using AutoMapper;
using Events.Specialization;
using MassTransit;

namespace Appointments.Consumers.Consumers.SpecializationConsumers
{
    public class UpdateSpecializationConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<SpecializationUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<SpecializationUpdated> context)
        {
            await _unitOfWork.SpecializationRepository.UpdateAsync(_mapper.Map<Specialization>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
