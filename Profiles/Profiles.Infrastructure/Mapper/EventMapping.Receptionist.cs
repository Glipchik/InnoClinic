using AutoMapper;
using Events.Receptionist;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddReceptionistEventMapping()
        {
            CreateMap<Receptionist, ReceptionistCreated>();
            CreateMap<Receptionist, ReceptionistUpdated>();
        }
    }
}
