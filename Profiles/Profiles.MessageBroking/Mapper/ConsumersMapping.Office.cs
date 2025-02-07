using AutoMapper;
using Events.Office;
using Profiles.Domain.Entities;

namespace Profiles.Consumers.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddOfficeMapping()
        {
            CreateMap<OfficeCreated, Office>();
            CreateMap<OfficeUpdated, Office>();
        }
    }
}
