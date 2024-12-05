using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
