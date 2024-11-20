using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _entities;

        public GenericRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);

            _entities = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task CreateAsync(T entity)
        {
            await _entities.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _entities.DeleteOneAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await _entities.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
