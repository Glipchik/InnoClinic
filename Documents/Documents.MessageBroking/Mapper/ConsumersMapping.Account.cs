using Documents.Domain.Entities;
using AutoMapper;
using Events.Account;

namespace Results.MessageBroking.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddAccountMapping()
        {
            CreateMap<AccountCreated, Account>();
        }
    }
}
