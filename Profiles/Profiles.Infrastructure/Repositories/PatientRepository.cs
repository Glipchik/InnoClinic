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

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var patient = await _context.Set<Patient>().FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
            _context.Set<Patient>().Remove(patient);
            
            var relatedAccount = await _context.Set<Account>().FirstOrDefaultAsync(a => a.Id == patient.AccountId, cancellationToken: cancellationToken);
            _context.Set<Account>().Remove(relatedAccount);
        }

        public async override Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .Include(p => p.Account)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Patient> GetAsync(Guid id, CancellationToken cancellationToken, bool isIncluded = true)
        {
            if (isIncluded)
            {
                return await _context.Set<Patient>().AsNoTracking()
                    .Include(p => p.Account)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
            }

            return await _context.Set<Patient>().AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
        }
    }
}
