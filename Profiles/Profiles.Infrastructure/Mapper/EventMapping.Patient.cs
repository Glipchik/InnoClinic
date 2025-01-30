using AutoMapper;
using Events.Patient;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddPatientEventMapping()
        {
            CreateMap<Patient, PatientCreated>();
            CreateMap<Patient, PatientUpdated>();
        }
    }
}
