using Documents.Domain.Entities;

namespace Documents.Domain.Repositories.Abstractions
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
