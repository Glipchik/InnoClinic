using Appointments.Domain.Entities;
using AutoMapper;
using Events.ServiceCategory;

namespace Appointments.Consumers.Mapper
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
