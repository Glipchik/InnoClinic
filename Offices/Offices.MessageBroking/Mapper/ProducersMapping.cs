using AutoMapper;

namespace Offices.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddOfficeMapping();

        public ProducersMapping()
        {
            AddOfficeMapping();
        }
    }
}
