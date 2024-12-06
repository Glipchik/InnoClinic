using Services.API.DTOs;
using Services.Application.Models;

namespace Services.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddServiceCategoryMapping()
        {
            CreateMap<UpdateServiceCategoryDto, UpdateServiceCategoryModel>();
            CreateMap<ServiceCategoryModel, ServiceCategoryDto>();
            CreateMap<CreateServiceCategoryDto, CreateServiceCategoryModel>();
        }
    }
}
