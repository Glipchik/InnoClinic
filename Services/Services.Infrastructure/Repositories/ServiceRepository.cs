using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Service>().AsNoTracking()
                .Include(s => s.Specialization)
                .Include(s => s.ServiceCategory)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Service>> GetAllAsync(Guid? serviceCategoryId, Guid? specializationId, bool? isActive, CancellationToken cancellationToken)
        {
            var query = _context.Set<Service>().AsNoTracking()
                .Include(s => s.Specialization)
                .Include(s => s.ServiceCategory)
                .AsQueryable();

            if (serviceCategoryId.HasValue)
            {
                query = query.Where(s => s.ServiceCategoryId == serviceCategoryId.Value);
            }

            if (specializationId.HasValue)
            {
                query = query.Where(s => s.SpecializationId == specializationId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(s => s.IsActive == isActive.Value);
            }

            return await query.ToListAsync(cancellationToken);
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
