using AutoMapper;
using Events.Doctor;
using Events.ServiceCategory;
using Services.Domain.Entities;

namespace Services.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddServiceCategoryEventMapping()
        {
            CreateMap<ServiceCategory, ServiceCategoryCreated>();
            CreateMap<ServiceCategory, ServiceCategoryUpdated>();
        }
    }
}
