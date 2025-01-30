using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Events.ServiceCategory;
using Events.Specialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public ServiceCategoryRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public override async Task CreateAsync(ServiceCategory entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<ServiceCategory>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<ServiceCategoryCreated>(entity), cancellationToken);
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Entity with id {id} to delete is null.");

            _context.Set<ServiceCategory>().Remove(entityToDelete);

            await _publishEndpoint.Publish(new ServiceCategoryDeleted() { Id = id }, cancellationToken);
        }

        public virtual async Task UpdateAsync(ServiceCategory entity, CancellationToken cancellationToken)
        {
            _context.Set<ServiceCategory>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<ServiceCategoryUpdated>(entity), cancellationToken);
        }

        public async override Task<IEnumerable<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<ServiceCategory>().AsNoTracking()
                .Include(sc => sc.Services)
                .ToListAsync(cancellationToken);
        }

        public async override Task<ServiceCategory> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<ServiceCategory>().AsNoTracking()
                .Include(sc => sc.Services)
                .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken: cancellationToken);
        }
    }
}
