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
    public class OfficeRepository: GenericRepository<Office>, IOfficeRepository
    {
        public OfficeRepository(MongoDbContext<Office> mongoDbContext):
            base(mongoDbContext)
        {
        }

        public override async Task DeleteAsync(string id)
        {
            await _mongoDbContext.Entities.UpdateOneAsync(Builders<Office>.Filter.Eq(e => e.Id, id), Builders<Office>.Update.Set(e => e.IsActive, true));
        }
    }
}
