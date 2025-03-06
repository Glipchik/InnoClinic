using AutoMapper;

namespace Documents.API.Mapper
{
    public partial class ApiMapping : Profile
    {
        partial void AddResultMapping();

        public ApiMapping()
        {
            AddResultMapping();
        }
    }
}
