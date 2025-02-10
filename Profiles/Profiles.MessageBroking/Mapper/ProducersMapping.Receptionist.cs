using AutoMapper;
using Events.Receptionist;
using Profiles.Domain.Entities;

namespace Profiles.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddReceptionistMapping()
        {
            CreateMap<Receptionist, ReceptionistCreated>();
            CreateMap<Receptionist, ReceptionistUpdated>();
        }
    }
}
