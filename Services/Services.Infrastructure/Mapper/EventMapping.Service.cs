using AutoMapper;
using Events.Service;
using Services.Domain.Entities;

namespace Services.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddServiceEventMapping()
        {
            CreateMap<Service, ServiceCreated>();
            CreateMap<Service, ServiceUpdated>();
        }
    }
}
