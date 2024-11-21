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
    public class DoctorRepository: GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MongoDbContext<Doctor> mongoDbContext)
            : base(mongoDbContext)
        {
        }

        public override async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _mongoDbContext.Entities.UpdateOneAsync(Builders<Doctor>.Filter.Eq(e => e.Id, id), Builders<Doctor>.Update.Set(e => e.Status, "Inactive"), cancellationToken: cancellationToken);
        }
    }
}
