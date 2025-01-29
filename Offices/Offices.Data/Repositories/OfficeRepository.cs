using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Events.Office;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories.Abstractions;
using Offices.Domain.Exceptions;
using SharpCompress.Common;

namespace Offices.Data.Repositories
{
    public class OfficeRepository: GenericRepository<Office>, IOfficeRepository
    {
        protected readonly IMongoCollection<Doctor> _doctorCollection;
        protected readonly IMongoCollection<Receptionist> _receptionistCollection;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public OfficeRepository(MongoDbContext mongoDbContext, IPublishEndpoint publishEndpoint, IMapper mapper) :
            base(mongoDbContext)
        {
            _doctorCollection = mongoDbContext.Database.GetCollection<Doctor>(typeof(Doctor).Name);
            _receptionistCollection = mongoDbContext.Database.GetCollection<Receptionist>(typeof(Receptionist).Name);
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _collection.UpdateOneAsync(Builders<Office>.Filter.Eq(e => e.Id, id), Builders<Office>.Update.Set(e => e.IsActive, false), cancellationToken: cancellationToken);

            var updatedOffice = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Updated office {id} is null.");

            OfficeUpdated officeUpdatedEvent = _mapper.Map<OfficeUpdated>(updatedOffice);
            officeUpdatedEvent.Id = updatedOffice.Id.ToString();

            await _publishEndpoint.Publish(officeUpdatedEvent, cancellationToken);
        }


        public override async Task<Office> UpdateAsync(Office entity, CancellationToken cancellationToken)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);

            var updatedOffice = await GetAsync(entity.Id, cancellationToken)
                ?? throw new ArgumentNullException($"Updated office {entity.Id} is null.");


            OfficeUpdated officeUpdatedEvent = _mapper.Map<OfficeUpdated>(updatedOffice);
            officeUpdatedEvent.Id = updatedOffice.Id.ToString();

            await _publishEndpoint.Publish(officeUpdatedEvent, cancellationToken);

            return entity;
        }

        public override async Task<Office> CreateAsync(Office entity, CancellationToken cancellationToken)
        {
            entity.Id = Guid.NewGuid();
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);


            OfficeCreated officeCreatedEvent = _mapper.Map<OfficeCreated>(entity);
            officeCreatedEvent.Id = entity.Id.ToString();

            await _publishEndpoint.Publish(officeCreatedEvent, cancellationToken);

            return entity;
        }
    }
}
