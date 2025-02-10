using AutoMapper;
using Events.Doctor;
using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddDoctorMapping()
        {
            CreateMap<Doctor, DoctorCreated>();
            CreateMap<Doctor, DoctorUpdated>();
        }
    }
}
