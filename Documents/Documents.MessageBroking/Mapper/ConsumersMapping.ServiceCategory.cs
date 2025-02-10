using Documents.Domain.Entities;
using AutoMapper;
using Events.ServiceCategory;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddServiceCategoryMapping()
        {
            CreateMap<ServiceCategoryCreated, ServiceCategory>();
            CreateMap<ServiceCategoryUpdated, ServiceCategory>();
        }
    }
}
