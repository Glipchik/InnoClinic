using Appointments.Domain.Entities;
using AutoMapper;
using Events.Patient;

namespace Appointments.Consumers.Mapper
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
