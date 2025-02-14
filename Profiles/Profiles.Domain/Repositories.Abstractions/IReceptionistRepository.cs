using Profiles.Domain.Entities;
using Profiles.Domain.Models;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IReceptionistRepository : IGenericRepository<Receptionist>
    {
        Task<Receptionist> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
        Task<PaginatedList<Receptionist>> GetAllAsync(Guid? OfficeId,
            CancellationToken cancellationToken,
            int pageIndex = 1, int pageSize = 10);

        Task<IEnumerable<Receptionist>> GetAllAsync(Guid? OfficeId,
            CancellationToken cancellationToken);
    }
}