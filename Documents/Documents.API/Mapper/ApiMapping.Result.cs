using Documents.API.DTOs;
using Documents.Application.Models;

namespace Documents.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddResultMapping()
        {
            CreateMap<CreateResultDto, CreateResultModel>();
            CreateMap<UpdateResultDto, UpdateResultModel>();
        }
    }
}
