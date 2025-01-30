using Profiles.API.DTOs;
using Profiles.Application.Models;
using Profiles.Domain.Entities;
using System;

namespace Profiles.API.Mapper;

public partial class ApiMapping
{
    partial void AddAccountMapping()
    {
        CreateMap<AccountModel, AccountDto>();
        CreateMap<CreateAccountDto, CreateAccountModel>();
        CreateMap<CreateAccountModel, CreateAccountDto>();
        CreateMap<Account, AuthorizationAccountModel>();
        CreateMap<AuthorizationAccountModel, Account>();
    }
}