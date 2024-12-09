using Services.API.DTOs;
using Services.Application.Models;

namespace Services.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddSpecializationMapping()
        {
            CreateMap<UpdateSpecializationDto, UpdateSpecializationModel>();
            CreateMap<SpecializationModel, SpecializationDto>();
            CreateMap<CreateSpecializationDto, CreateSpecializationModel>();
        }
    }
}
