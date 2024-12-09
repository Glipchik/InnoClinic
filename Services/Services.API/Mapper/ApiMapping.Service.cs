using Services.API.DTOs;
using Services.Application.Models;

namespace Services.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddServiceMapping()
        {
            CreateMap<UpdateServiceDto, UpdateServiceModel>();
            CreateMap<ServiceModel, ServiceDto>();
            CreateMap<CreateServiceDto, CreateServiceModel>();
        }
    }
}
