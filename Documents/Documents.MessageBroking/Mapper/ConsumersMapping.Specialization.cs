using Documents.Domain.Entities;
using AutoMapper;
using Events.Specialization;

namespace Results.MessageBroking.Mapper
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
