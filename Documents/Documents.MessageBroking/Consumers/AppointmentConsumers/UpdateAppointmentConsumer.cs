using AutoMapper;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Appointment;
using MassTransit;

namespace Documents.MessageBroking.Consumers.AppointmentConsumers
{
    public class UpdateAppointmentConsumer(IUnitOfWork unitOfWork, IMapper mapper) : IConsumer<AppointmentUpdated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Consume(ConsumeContext<AppointmentUpdated> context)
        {
            _unitOfWork.AppointmentRepository.Update(_mapper.Map<Appointment>(context.Message), CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
