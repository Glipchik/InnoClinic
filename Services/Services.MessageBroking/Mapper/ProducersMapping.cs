using AutoMapper;

namespace Services.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddServiceCategoryMapping();
        partial void AddSpecializationMapping();
        partial void AddServiceMapping();

        public ProducersMapping()
        {
            AddServiceCategoryMapping();
            AddSpecializationMapping();
            AddServiceMapping();
        }
    }
}
