using Documents.Application.Models;
using Documents.Domain.Entities;

namespace Documents.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddResultMapping()
        {
            CreateMap<CreateResultModel, Result>();
            CreateMap<Result, ResultModel>();
            CreateMap<UpdateResultModel, Result>();
        }
    }
}
