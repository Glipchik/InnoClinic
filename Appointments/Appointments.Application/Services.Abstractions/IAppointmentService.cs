using Appointments.Application.Models;

namespace Appointments.Application.Services.Abstractions
{
    public interface IAppointmentService
    {
        Task<AppointmentModel> Create(CreateAppointmentModel createAppointmentModel, CancellationToken cancellationToken);
        Task<AppointmentModel> Update(UpdateAppointmentModel updateAppointmentModel, CancellationToken cancellationToken);
        Task<AppointmentModel> Get(Guid Id, CancellationToken cancellationToken);
        Task<IEnumerable<AppointmentModel>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<AppointmentModel>> GetAllByPatientId(Guid patientId, CancellationToken cancellationToken);
        Task<IEnumerable<AppointmentModel>> GetAllByDoctorId(Guid doctorId, CancellationToken cancellationToken);
        Task<AppointmentModel> Approve(Guid appointmentId, CancellationToken cancellationToken);
        Task<IEnumerable<TimeSlotModel>> GetDoctorsSchedule(Guid doctorId, DateOnly date, CancellationToken cancellationToken);
    }
}
