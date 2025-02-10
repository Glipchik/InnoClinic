using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Events.Doctor;
using MassTransit;

namespace Appointments.MessageBroking.Consumers.DoctorConsumers
{
    public class DeactivateDoctorConsumer(IUnitOfWork unitOfWork) : IConsumer<DoctorDeactivated>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<DoctorDeactivated> context)
        {
            await _unitOfWork.DoctorRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
