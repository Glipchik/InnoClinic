using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Models
{
    public class AppointmentModel
    {
        public required Guid Id { get; set; }
        public required Guid PatientId { get; set; }
        public required Guid DoctorId { get; set; }
        public required Guid ServiceId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
        public required bool IsApproved { get; set; }
    }

    public class CreateAppointmentModel
    {
        public required Guid PatientId { get; set; }
        public required Guid DoctorId { get; set; }
        public required Guid ServiceId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
    }

    public class UpdateAppointmentModel
    {
        public required Guid Id { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
    }

    public class AppointmentsScheduleModel
    {
        public required DateTime Start { get; set; }
        public required DateTime Finish { get; set; }
    }
}
