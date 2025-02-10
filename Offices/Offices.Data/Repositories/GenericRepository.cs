using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using Offices.Domain.Models;
using SharpCompress.Common;
using System.Numerics;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(MongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.Database.GetCollection<T>(typeof(T).Name);
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<PaginatedList<T>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var entities = _collection.Find(_ => true);

            var paginatedEntities = (await entities.ToListAsync(cancellationToken: cancellationToken))
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await entities.CountDocumentsAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<T>(paginatedEntities, pageIndex, totalPages);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await (await _collection.FindAsync(d => d.Id == id, cancellationToken: cancellationToken)).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);

            return entity;
        }
    }
}
