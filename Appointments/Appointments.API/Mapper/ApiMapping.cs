using AutoMapper;

namespace Appointments.API.Mapper
{
    public partial class ApiMapping : Profile
    {
        partial void AddAppointmentMapping();

        public ApiMapping()
        {
            AddAppointmentMapping();
        }
    }
}
