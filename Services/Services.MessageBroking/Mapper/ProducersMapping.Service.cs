using AutoMapper;
using Events.Service;
using Services.Domain.Entities;

namespace Services.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddServiceMapping()
        {
            CreateMap<Service, ServiceCreated>();
            CreateMap<Service, ServiceUpdated>();
        }
    }
}
