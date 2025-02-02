using AutoMapper;
using Events.Specialization;
using Services.Domain.Entities;

namespace Services.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddSpecializationMapping()
        {
            CreateMap<Specialization, SpecializationCreated>();
            CreateMap<Specialization, SpecializationUpdated>();
        }
    }
}
