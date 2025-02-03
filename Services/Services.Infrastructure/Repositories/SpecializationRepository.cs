using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly AppDbContext _context;

        public SpecializationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Specialization> UpdateAsync(Specialization entity, CancellationToken cancellationToken)
        {
            _context.Set<Specialization>().Update(entity);

            return entity;
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
    }
}
