using Offices.Data.Entities;
using Offices.Domain.Models;

namespace Offices.Data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<PaginatedList<T>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
    }
}
