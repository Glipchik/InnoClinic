using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Doctor> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await GetAsync(id, cancellationToken);
            ArgumentNullException.ThrowIfNull(doctorToDelete, nameof(doctorToDelete));
            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);
            return doctorToDelete;
        }

        public async Task<Doctor?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);
        }

        public override async Task<Doctor?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
