﻿using Appointments.Domain.Entities;

namespace Appointments.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
