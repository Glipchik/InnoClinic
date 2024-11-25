using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using SharpCompress.Common;

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
            var entityToUpdate = await _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (entityToUpdate == null)
            {
                throw new NotFoundException($"Object with id {id} not found!");
            }
            await _collection.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await (await _collection.FindAsync(d => d.Id == id, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"Object with id {id} not found!");
            }
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            var entityToUpdate = await _collection.Find(e => e.Id == entity.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (entityToUpdate == null)
            {
                throw new NotFoundException($"Object with id {entity.Id} not found!");
            }
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }
    }
}
