using AutoMapper;

namespace Profiles.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddAccountMapping();
        partial void AddDoctorMapping();
        partial void AddPatientMapping();
        partial void AddReceptionistMapping();

        public ProducersMapping()
        {
            AddAccountMapping();
            AddDoctorMapping();
            AddPatientMapping();
            AddReceptionistMapping();
        }
    }
}
