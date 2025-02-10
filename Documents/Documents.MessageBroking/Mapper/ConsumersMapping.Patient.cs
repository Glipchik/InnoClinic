using Documents.Domain.Entities;
using AutoMapper;
using Events.Patient;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddPatientMapping()
        {
            CreateMap<PatientCreated, Patient>();
            CreateMap<PatientUpdated, Patient>();
        }
    }
}
