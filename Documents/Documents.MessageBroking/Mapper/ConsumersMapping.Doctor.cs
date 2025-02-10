using Documents.Domain.Entities;
using AutoMapper;
using Events.Doctor;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddDoctorMapping()
        {
            CreateMap<DoctorCreated, Doctor>();
            CreateMap<DoctorUpdated, Doctor>();
        }
    }
}
