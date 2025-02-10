using AutoMapper;

namespace Results.MessageBroking.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddResultEventMapping();

        public EventMapping()
        {
            AddResultEventMapping();
        }
    }
}
