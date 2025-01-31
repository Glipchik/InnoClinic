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

        public OfficeRepository(MongoDbContext mongoDbContext) :
            base(mongoDbContext)
        {
            _doctorCollection = mongoDbContext.Database.GetCollection<Doctor>(typeof(Doctor).Name);
            _receptionistCollection = mongoDbContext.Database.GetCollection<Receptionist>(typeof(Receptionist).Name);
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _collection.UpdateOneAsync(Builders<Office>.Filter.Eq(e => e.Id, id), Builders<Office>.Update.Set(e => e.IsActive, false), cancellationToken: cancellationToken);

            var updatedOffice = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Updated office {id} is null.");
        }


        public override async Task<Office> UpdateAsync(Office entity, CancellationToken cancellationToken)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);

            var updatedOffice = await GetAsync(entity.Id, cancellationToken)
                ?? throw new ArgumentNullException($"Updated office {entity.Id} is null.");

            return entity;
        }

        public override async Task<Office> CreateAsync(Office entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

            return entity;
        }
    }
}
