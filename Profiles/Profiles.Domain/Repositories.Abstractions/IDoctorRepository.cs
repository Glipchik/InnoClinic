using Profiles.Domain.Entities;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}