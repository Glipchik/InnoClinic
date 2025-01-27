using Appointments.Application.Models;
using Appointments.Domain.Entities;

namespace Appointments.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddAppointmentMapping()
        {
            CreateMap<Appointment, AppointmentModel>();
            CreateMap<CreateAppointmentModel, Appointment>();
            CreateMap<UpdateAppointmentModel, Appointment>();
            CreateMap<Appointment, UpdateAppointmentModel>();
        }
    }
}
