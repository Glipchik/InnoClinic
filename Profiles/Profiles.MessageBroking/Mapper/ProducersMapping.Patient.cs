using AutoMapper;
using Events.Patient;
using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddPatientMapping()
        {
            CreateMap<Patient, PatientCreated>();
            CreateMap<Patient, PatientUpdated>();
        }
    }
}
