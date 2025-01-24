using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Appointments.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Service> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var serviceToDelete = await GetAsync(id, cancellationToken);
            ArgumentNullException.ThrowIfNull(serviceToDelete, nameof(serviceToDelete));
            serviceToDelete.IsActive = false;
            _context.Set<Service>().Update(serviceToDelete);
            return serviceToDelete;
        }
    }
}
