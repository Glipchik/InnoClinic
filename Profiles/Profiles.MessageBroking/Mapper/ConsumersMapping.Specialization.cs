using AutoMapper;
using Events.Specialization;
using Profiles.Domain.Entities;

namespace Profiles.Consumers.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddSpecializationMapping()
        {
            CreateMap<SpecializationCreated, Specialization>();
            CreateMap<SpecializationUpdated, Specialization>();
        }
    }
}
