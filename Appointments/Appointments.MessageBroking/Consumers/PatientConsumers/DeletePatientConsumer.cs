using Appointments.Domain.Repositories.Abstractions;
using Events.Patient;
using MassTransit;

namespace Appointments.Consumers.Consumers.PatientConsumers
{
    public class DeletePatientConsumer(IUnitOfWork unitOfWork) : IConsumer<PatientDeleted>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<PatientDeleted> context)
        {
            await _unitOfWork.PatientRepository.DeleteAsync(context.Message.Id, CancellationToken.None);
            await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
