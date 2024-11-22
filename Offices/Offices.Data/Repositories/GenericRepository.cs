using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(MongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.Database.GetCollection<T>(typeof(T).Name);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public virtual async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken)
        {
            return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }
    }
}
