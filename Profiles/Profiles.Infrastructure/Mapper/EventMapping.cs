using AutoMapper;

namespace Profiles.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddAccountEventMapping();
        partial void AddDoctorEventMapping();
        partial void AddPatientEventMapping();
        partial void AddReceptionistEventMapping();

        public EventMapping()
        {
            AddAccountEventMapping();
            AddDoctorEventMapping();
            AddPatientEventMapping();
            AddReceptionistEventMapping();
        }
    }
}
