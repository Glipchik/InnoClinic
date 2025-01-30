using AutoMapper;
using Events.Specialization;
using Services.Domain.Entities;

namespace Services.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddSpecializationEventMapping()
        {
            CreateMap<Specialization, SpecializationCreated>();
            CreateMap<Specialization, SpecializationUpdated>();
        }
    }
}
