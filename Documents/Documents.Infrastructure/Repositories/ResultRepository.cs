using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Documents.Infrastructure.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        private readonly AppDbContext _context;

        public ResultRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken)
        {
            return await _context.Set<Result>().AsNoTracking()
                .Include(r => r.Appointment)
                .Where(r => r.Appointment.DoctorId == doctorId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Result>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Set<Result>().AsNoTracking()
                .Include(r => r.Appointment)
                .Where(r => r.Appointment.PatientId == patientId).ToListAsync(cancellationToken);
        }
    }
}
