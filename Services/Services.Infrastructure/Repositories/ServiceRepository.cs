using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Exceptions;
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

        public async Task<IEnumerable<Service>> GetActiveServicesByCategoryIdAsync(Guid serviceCategoryId, CancellationToken cancellationToken)
        {
            return await _context.Set<Service>().AsNoTracking().Where(s => s.ServiceCategoryId == serviceCategoryId && s.IsActive == true).ToListAsync(cancellationToken);
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await _context.Set<Service>().FindAsync(id) ?? throw new NotFoundException($"Service with id: {id} not found. Can't delete.");
            serviceToDelete.IsActive = false;
            _context.Set<Service>().Update(serviceToDelete);
        }
    }
}
