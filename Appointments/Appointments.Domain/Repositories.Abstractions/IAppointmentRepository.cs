﻿using Appointments.Domain.Entities;

namespace Appointments.Domain.Repositories.Abstractions
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetAllApprovedByDoctorIdFromNowAsync(Guid doctorId, CancellationToken cancellationToken);
    }
}
