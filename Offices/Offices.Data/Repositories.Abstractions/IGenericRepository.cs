using Offices.Data.Entities;

namespace Offices.Data.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task CreateAsync(T entity);
    }
}
