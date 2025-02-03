using AutoMapper;

namespace Profiles.Consumers.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddSpecializationMapping();
        partial void AddOfficeMapping();

        public ConsumersMapping()
        {
            AddOfficeMapping();
            AddSpecializationMapping();
        }
    }
}
