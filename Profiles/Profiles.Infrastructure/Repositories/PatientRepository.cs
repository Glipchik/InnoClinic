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
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .Include(p => p.Account)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Patient> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
        }
    }
}
