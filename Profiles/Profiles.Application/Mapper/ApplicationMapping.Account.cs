using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Mapper
{
    partial class ApplicationMapping : Profile
    {
        partial void AddAccountMapping()
        {
            CreateMap<Account, AccountModel>();
            CreateMap<AccountModel, Account>();
            CreateMap<CreateAccountModel, Account>();
            CreateMap<CreateAccountModel, CreateAccountAuthorizationServerModel>();
            CreateMap<CreateAccountFromAuthServerModel, Account>();
            CreateMap<CreateAccountAuthorizationServerModel, Account>();
        }
    }
}
