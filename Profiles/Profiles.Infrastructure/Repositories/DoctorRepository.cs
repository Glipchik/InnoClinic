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
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await _context.Set<Doctor>().FindAsync(id);
            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);
        }

        public async override Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .Include(d => d.Account)
                .Include(d => d.Office)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Doctor> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .Include(d => d.Account)
                .Include(d => d.Office)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
        }
    }
}
