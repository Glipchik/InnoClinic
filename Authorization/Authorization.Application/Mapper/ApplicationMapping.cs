using AutoMapper;

namespace Authorization.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddAccountMapping();

        public ApplicationMapping()
        {
            AddAccountMapping();
        }
    }
}