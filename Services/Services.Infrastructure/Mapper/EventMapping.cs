using AutoMapper;
using Events.Doctor;
using Services.Domain.Entities;

namespace Services.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddServiceCategoryEventMapping();
        partial void AddSpecializationEventMapping();
        partial void AddServiceEventMapping();

        public EventMapping()
        {
            AddServiceCategoryEventMapping();
            AddSpecializationEventMapping();
            AddServiceEventMapping();
        }
    }
}
