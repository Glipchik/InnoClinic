using Offices.Data.Entities;

namespace Offices.Data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetAsync(string id, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
    }
}
