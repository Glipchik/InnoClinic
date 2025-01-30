using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Events.Service;
using Events.Service;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        public ServiceRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await GetAsync(id, cancellationToken);
            serviceToDelete.IsActive = false;

            _context.Set<Service>().Update(serviceToDelete);

            await _publishEndpoint.Publish(_mapper.Map<ServiceUpdated>(serviceToDelete), cancellationToken);
        }

        public override async Task CreateAsync(Service entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Service>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<ServiceCreated>(entity), cancellationToken);
        }

        public virtual async Task UpdateAsync(Service entity, CancellationToken cancellationToken)
        {
            _context.Set<Service>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<ServiceUpdated>(entity), cancellationToken);
        }

        public async override Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Service>().AsNoTracking()
                .Include(s => s.Specialization)
                .Include(s => s.ServiceCategory)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Service> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Service>().AsNoTracking()
                .Include(s => s.Specialization)
                .Include(s => s.ServiceCategory)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
        }
    }
}
