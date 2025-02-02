using AutoMapper;
using Events.Doctor;
using MassTransit;
using Offices.Data.Repositories.Abstractions;

namespace Offices.MessageBroking.Consumers.DoctorConsumers
{
    public class DeactivateDoctorConsumer(IDoctorRepository doctorRepository) : IConsumer<DoctorDeactivated>
    {
        private readonly IDoctorRepository _doctorRepository = doctorRepository;

        public async Task Consume(ConsumeContext<DoctorDeactivated> context)
        {
            var doctorToUpdate = await _doctorRepository.GetAsync(context.Message.Id, CancellationToken.None)
                ?? throw new ArgumentNullException("Doctor is null.");

            doctorToUpdate.Status = Data.Enums.DoctorStatus.Inactive;
            await _doctorRepository.UpdateAsync(doctorToUpdate, CancellationToken.None);
        }
    }
}
