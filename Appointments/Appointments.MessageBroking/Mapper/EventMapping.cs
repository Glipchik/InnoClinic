using AutoMapper;

namespace Appointments.Consumers.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddAppointmentEventMapping();

        public EventMapping()
        {
            AddAppointmentEventMapping();
        }
    }
}
