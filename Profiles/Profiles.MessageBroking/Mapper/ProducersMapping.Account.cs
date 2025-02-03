using AutoMapper;
using Events.Account;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.MessageBroking.Mapper
{
    public partial class ProducersMapping : Profile
    {
        partial void AddAccountMapping()
        {
            CreateMap<Account, AccountCreated>();
        }
    }
}
