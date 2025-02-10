using Documents.Domain.Entities;
using AutoMapper;
using Events.Service;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddServiceMapping()
        {
            CreateMap<ServiceCreated, Service>();
            CreateMap<ServiceUpdated, Service>();
        }
    }
}
