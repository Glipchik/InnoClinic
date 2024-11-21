using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Data.Repositories
{
    public class OfficeRepository: IGenericRepository<Office>
    {
        private readonly MongoDbContext<Office> _mongoDbContext;

        public OfficeRepository(MongoDbContext<Office> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateAsync(Office entity)
        {
            await _mongoDbContext.Entities.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _mongoDbContext.Entities.UpdateOneAsync(Builders<Office>.Filter.Eq(e => e.Id, id), Builders<Office>.Update.Set(e => e.IsActive, true));
        }

        public async Task<IEnumerable<Office>> GetAllAsync()
        {
            return await _mongoDbContext.Entities.Find(o => o.IsActive == true).ToListAsync();
        }

        public async Task<Office> GetAsync(string id)
        {
            return await _mongoDbContext.Entities.Find(e => e.Id == id && e.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Office entity)
        {
            await _mongoDbContext.Entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
