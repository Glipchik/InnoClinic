using AutoMapper;
using Events.Doctor;
using Services.Domain.Entities;

namespace Services.Consumers.Mapper
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
