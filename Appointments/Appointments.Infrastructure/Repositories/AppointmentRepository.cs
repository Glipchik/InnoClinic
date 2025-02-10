using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Appointments.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Appointment> ApproveAsync(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointmentToApprove = await GetAsync(appointmentId, cancellationToken)
                ?? throw new ArgumentNullException("Appointment to approve is null.");

            appointmentToApprove.IsApproved = true;
            await UpdateAsync(appointmentToApprove, cancellationToken);

            return appointmentToApprove;
        }

        public override async Task<Appointment> CreateAsync(Appointment entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Appointment>().AddAsync(entity, cancellationToken);

            return entity;
        }

        public virtual async Task<Appointment> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Entity with id {id} to delete is null.");

            _context.Set<Appointment>().Remove(entityToDelete);

            return entityToDelete;
        }

        public async Task<IEnumerable<Appointment>> GetAllApprovedByDoctorIdAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Where(a => a.DoctorId == doctorId && a.Date == date && a.IsApproved == true).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Service.ServiceCategory)
                .Where(a => a.DoctorId == doctorId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Service.ServiceCategory)
                .Where(a => a.PatientId == patientId).ToListAsync(cancellationToken);
        }

        public override async Task<Appointment?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .Include(a => a.Service.ServiceCategory)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
