using Documents.Domain.Entities;
using AutoMapper;
using Events.Appointment;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddAppointmentMapping()
        {
            CreateMap<AppointmentCreated, Appointment>();
            CreateMap<AppointmentUpdated, Appointment>();
        }
    }
}
