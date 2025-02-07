using Profiles.Domain.Entities;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IReceptionistRepository : IGenericRepository<Receptionist>
    {
        Task<Receptionist> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}