using Documents.Domain.Repositories.Abstractions;
using Events.Account;
using Events.Appointment;
using MassTransit;

namespace Documents.MessageBroking.Consumers.AppointmentConsumers
{
    public class DeleteAppointmentConsumer(IUnitOfWork unitOfWork) : IConsumer<AppointmentDeleted>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<AppointmentDeleted> context)
        {
            await _unitOfWork.AppointmentRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
