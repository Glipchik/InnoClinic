using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly MongoDbContext<T> _mongoDbContext;

        public GenericRepository(MongoDbContext<T> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _mongoDbContext.Entities.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public virtual async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _mongoDbContext.Entities.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _mongoDbContext.Entities.Find(_ => true).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken)
        {
            return await _mongoDbContext.Entities.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await _mongoDbContext.Entities.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }
    }
}
