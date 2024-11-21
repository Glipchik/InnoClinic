using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MongoDbContext<T> _mongoDbContext;

        public GenericRepository(MongoDbContext<T> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateAsync(T entity)
        {
            await _mongoDbContext.Entities.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _mongoDbContext.Entities.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _mongoDbContext.Entities.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await _mongoDbContext.Entities.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _mongoDbContext.Entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
