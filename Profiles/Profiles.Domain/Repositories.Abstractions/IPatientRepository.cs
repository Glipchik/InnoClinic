using Profiles.Domain.Entities;
using Profiles.Domain.Models;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
        Task<PaginatedList<Patient>> GetAllAsync(
            CancellationToken cancellationToken,
            int pageIndex = 1, int pageSize = 10);
    }
}