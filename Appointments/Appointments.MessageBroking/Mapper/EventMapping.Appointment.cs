using Appointments.Domain.Entities;
using AutoMapper;
using Events.Appointment;

namespace Appointments.Consumers.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddAppointmentEventMapping()
        {
            CreateMap<Appointment, AppointmentCreated>();
            CreateMap<Appointment, AppointmentUpdated>();
        }
    }
}
