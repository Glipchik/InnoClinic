using Offices.Data.Entities;

namespace Offices.Data.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task CreateAsync(T entity);
    }
}
