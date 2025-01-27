using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Appointments.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Update(appointmentToApprove, cancellationToken);
            return appointmentToApprove;
        }

        public async Task<IEnumerable<Appointment>> GetAllApprovedByDoctorIdAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking().Where(a => a.DoctorId == doctorId && a.Date == date).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking().Where(a => a.DoctorId == doctorId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking().Where(a => a.PatientId == patientId).ToListAsync(cancellationToken);
        }

        public override async Task<Appointment?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>().AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Doctor)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
