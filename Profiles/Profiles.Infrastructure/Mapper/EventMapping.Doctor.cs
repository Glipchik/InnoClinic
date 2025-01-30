using AutoMapper;
using Events.Doctor;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddDoctorEventMapping()
        {
            CreateMap<Doctor, DoctorCreated>();
            CreateMap<Doctor, DoctorUpdated>();
        }
    }
}
