using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.Repositories
{
    public class OfficeRepository : GenericRepository<Office>, IOfficeRepository
    {
        private readonly AppDbContext _context;

        public OfficeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var officeToDelete = await _context.Set<Office>().FindAsync(id);
            officeToDelete.IsActive = false;
            _context.Set<Office>().Update(officeToDelete);
        }
    }
}
