using AutoMapper;
using Events.Office;
using Offices.Data.Entities;

namespace Offices.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddOfficeMapping()
        {
            CreateMap<Office, OfficeCreated>();
            CreateMap<Office, OfficeUpdated>();
        }
    }
}
