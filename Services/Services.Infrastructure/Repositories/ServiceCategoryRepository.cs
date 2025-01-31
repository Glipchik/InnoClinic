using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        private readonly AppDbContext _context;

        public ServiceCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Entity with id {id} to delete is null.");

            _context.Set<ServiceCategory>().Remove(entityToDelete);
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
