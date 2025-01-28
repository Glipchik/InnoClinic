using Appointments.Domain.Entities;

namespace Appointments.Domain.Repositories.Abstractions
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetAllApprovedByDoctorIdAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken);
        Task<Appointment> ApproveAsync(Guid appointmentId, CancellationToken cancellationToken);
    }
}
