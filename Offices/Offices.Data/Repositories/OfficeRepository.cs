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
        protected readonly IMongoCollection<Doctor> _doctorCollection;
        protected readonly IMongoCollection<Receptionist> _receptionistCollection;

        public OfficeRepository(MongoDbContext mongoDbContext):
            base(mongoDbContext)
        {
            _doctorCollection = mongoDbContext.Database.GetCollection<Doctor>(typeof(Doctor).Name); ;
            _receptionistCollection = mongoDbContext.Database.GetCollection<Receptionist>(typeof(Receptionist).Name); ;
        }

        public async Task<bool> CheckIfThereAreDoctorsOrReceptionistsInOffice(string officeId, CancellationToken cancellationToken)
        {
            var areAnyDoctorsInOffice = await (await _doctorCollection.FindAsync(d => d.OfficeId == officeId, cancellationToken: cancellationToken)).AnyAsync(cancellationToken: cancellationToken);
            var areAnyReceptionistsInOffice = await (await _receptionistCollection.FindAsync(r => r.OfficeId == officeId, cancellationToken: cancellationToken)).AnyAsync(cancellationToken: cancellationToken);

            // If someone works in the office, returns true
            return areAnyDoctorsInOffice || areAnyReceptionistsInOffice;
        }

        public override async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _collection.UpdateOneAsync(Builders<Office>.Filter.Eq(e => e.Id, id), Builders<Office>.Update.Set(e => e.IsActive, true), cancellationToken: cancellationToken);
        }
    }
}
