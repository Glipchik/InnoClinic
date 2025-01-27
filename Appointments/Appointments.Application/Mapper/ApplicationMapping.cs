using AutoMapper;

namespace Appointments.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddAppointmentMapping();
        partial void AddPatientMapping();

        public ApplicationMapping()
        {
            AddAppointmentMapping();
            AddPatientMapping();
        }
    }
}