using Appointments.Domain.Entities;
using AutoMapper;
using Events.Service;

namespace Appointments.Consumers.Mapper
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
