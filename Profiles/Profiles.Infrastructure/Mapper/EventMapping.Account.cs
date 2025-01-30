using AutoMapper;
using Events.Account;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Mapper
{
    public partial class EventMapping : Profile
    {
        partial void AddAccountEventMapping()
        {
            CreateMap<Account, AccountCreated>();
        }
    }
}
