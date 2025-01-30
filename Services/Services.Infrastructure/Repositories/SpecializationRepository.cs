using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Events.Specialization;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public SpecializationRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var specializationToDelete = await GetAsync(id, cancellationToken);
            specializationToDelete.IsActive = false;
            _context.Set<Specialization>().Update(specializationToDelete);

            await _publishEndpoint.Publish(_mapper.Map<SpecializationUpdated>(specializationToDelete), cancellationToken);
        }

        public override async Task UpdateAsync(Specialization entity, CancellationToken cancellationToken)
        {
            _context.Set<Specialization>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<SpecializationUpdated>(entity), cancellationToken);
        }

        public async override Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Specialization>().AsNoTracking()
                .Include(s => s.Doctors)
                .Include(s => s.Services)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Specialization> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Specialization>().AsNoTracking()
                .Include(s => s.Doctors)
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
        }

        public override async Task CreateAsync(Specialization entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Specialization>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<SpecializationCreated>(entity), cancellationToken);
        }

    }
}
