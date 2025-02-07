using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;

namespace Profiles.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly IAccountRepository _accountRepository;

        public PatientRepository(AppDbContext context, IAccountRepository accountRepository) : base(context)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var patient = await  GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Patient with id {id} not found");

            _context.Set<Patient>().Remove(patient);
            
            await _accountRepository.DeleteAsync(patient.AccountId, id, cancellationToken);
        }

        public override async Task<Patient> CreateAsync(Patient entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Patient>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public override async Task<Patient> UpdateAsync(Patient entity, CancellationToken cancellationToken)
        {
            _context.Set<Patient>().Update(entity);
            return entity;
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

        public async Task<Patient> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken: cancellationToken);
        }
    }
}
