using Profiles.Domain.Entities;
using Profiles.Domain.Enums;
using Profiles.Domain.Models;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
        Task<PaginatedList<Doctor>> GetAllAsync(Guid? specializationId, DoctorStatus? status, Guid? OfficeId,
            CancellationToken cancellationToken,
            int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<Doctor>> GetAllAsync(Guid? specializationId, DoctorStatus? status, Guid? OfficeId,
            CancellationToken cancellationToken);
    }
}