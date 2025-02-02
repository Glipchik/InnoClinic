using Documents.Domain.Entities;
using AutoMapper;
using Events.Result;

namespace Results.MessageBroking.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddResultEventMapping()
        {
            CreateMap<Result, ResultCreated>();
            CreateMap<Result, ResultUpdated>();
        }
    }
}
