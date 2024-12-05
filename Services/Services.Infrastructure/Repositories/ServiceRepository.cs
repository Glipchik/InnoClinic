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
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _context;
        public ServiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await _context.Set<Service>().FindAsync(id);
            serviceToDelete.IsActive = false;
            _context.Set<Service>().Update(serviceToDelete);
        }
    }
}
