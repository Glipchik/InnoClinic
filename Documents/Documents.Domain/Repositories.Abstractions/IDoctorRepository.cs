using Documents.Domain.Entities;

namespace Documents.Domain.Repositories.Abstractions
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
