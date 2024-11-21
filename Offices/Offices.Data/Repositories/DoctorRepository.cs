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
    public class DoctorRepository: IGenericRepository<Doctor>
    {
        private readonly MongoDbContext<Doctor> _mongoDbContext;

        public DoctorRepository(MongoDbContext<Doctor> mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateAsync(Doctor entity)
        {
            await _mongoDbContext.Entities.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _mongoDbContext.Entities.UpdateOneAsync(Builders<Doctor>.Filter.Eq(e => e.Id, id), Builders<Doctor>.Update.Set(e => e.Status, "Inactive"));
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _mongoDbContext.Entities.Find(d => d.Status != "Inactive").ToListAsync();
        }

        public async Task<Doctor> GetAsync(string id)
        {
            return await _mongoDbContext.Entities.Find(d => d.Id == id && d.Status != "Inactive").FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Doctor entity)
        {
            await _mongoDbContext.Entities.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
    }
}
