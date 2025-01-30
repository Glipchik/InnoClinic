using Appointments.Domain.Entities;
using AutoMapper;
using Events.Specialization;

namespace Appointments.Consumers.Mapper
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
