using AutoMapper;
using Events.Service;
using Events.ServiceCategory;
using Services.Domain.Entities;

namespace Services.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddServiceCategoryMapping()
        {
            CreateMap<ServiceCategory, ServiceCategoryCreated>();
            CreateMap<ServiceCategory, ServiceCategoryUpdated>();
        }
    }
}
