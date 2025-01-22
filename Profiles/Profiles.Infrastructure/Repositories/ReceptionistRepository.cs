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
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {
        private readonly AppDbContext _context;

        public ReceptionistRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<IEnumerable<Receptionist>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Receptionist>().AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Office)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Receptionist> GetAsync(Guid id, CancellationToken cancellationToken, bool isIncluded = true)
        {
            if (isIncluded)
            {
                return await _context.Set<Receptionist>().AsNoTracking()
                    .Include(r => r.Account)
                    .Include(r => r.Office)
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            }

            return await _context.Set<Receptionist>().AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
        }
    }
}
