using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetActiveDoctorsBySpecializationIdAsync(Guid specializationId, CancellationToken cancellationToken);
    }
}
