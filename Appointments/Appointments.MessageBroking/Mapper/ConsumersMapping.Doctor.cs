using Appointments.Domain.Entities;
using AutoMapper;
using Events.Doctor;

namespace Appointments.Consumers.Mapper
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
