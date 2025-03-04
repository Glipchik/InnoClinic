﻿using Appointments.Domain.Entities;

namespace Appointments.Domain.Repositories.Abstractions
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
