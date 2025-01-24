using AutoMapper;

namespace Appointments.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddAppointmentMapping();

        public ApplicationMapping()
        {
            AddAppointmentMapping();
        }
    }
}