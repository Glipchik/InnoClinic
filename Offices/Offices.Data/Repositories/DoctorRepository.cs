using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Enums;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using SharpCompress.Common;

namespace Offices.Data.Repositories
{
    public class DoctorRepository: GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MongoDbContext mongoDbContext)
            : base(mongoDbContext)
        {
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _collection.UpdateOneAsync(Builders<Doctor>.Filter.Eq(e => e.Id, id), Builders<Doctor>.Update.Set(e => e.Status, DoctorStatus.Inactive), cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsFromOffice(Guid officeId, CancellationToken cancellationToken)
        {
            var cursor = await _collection.FindAsync(d => d.OfficeId == officeId && d.Status == DoctorStatus.AtWork, cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsFromOffice(Guid officeId, CancellationToken cancellationToken)
        {
            var cursor = await _collection.FindAsync(d => d.OfficeId == officeId, cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }
    }
}
