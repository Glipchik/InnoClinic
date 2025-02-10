using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Events.Doctor;
using MassTransit;

namespace Documents.MessageBroking.Consumers.DoctorConsumers
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
