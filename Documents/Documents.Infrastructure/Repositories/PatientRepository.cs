using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Patient?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking().FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken);
        }
    }
}
