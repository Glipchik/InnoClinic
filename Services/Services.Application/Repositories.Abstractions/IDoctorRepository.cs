using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Repositories.Abstractions
{
    public interface IDoctorRepository : IGeneralRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetActiveDoctorsWithSpecializationAsync(Guid specializationId, CancellationToken cancellationToken);
    }
}
