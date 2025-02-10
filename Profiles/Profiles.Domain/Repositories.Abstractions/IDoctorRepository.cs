using Profiles.Domain.Entities;
using Profiles.Domain.Enums;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
        Task<IEnumerable<Doctor>> GetAllAsync(Guid? specializationId, DoctorStatus? status, CancellationToken cancellationToken);
    }
}