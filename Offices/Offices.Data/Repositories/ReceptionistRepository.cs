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
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {
        public ReceptionistRepository(MongoDbContext mongoDbContext)
            : base(mongoDbContext)
        {
        }

        public async Task<IEnumerable<Receptionist>> GetActiveReceptionistsFromOffice(string officeId, CancellationToken cancellationToken)
        {
            var cursor = await _collection.FindAsync(r => r.OfficeId == officeId, cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }
    }
}
