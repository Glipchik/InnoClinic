using Profiles.Domain.Entities;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}