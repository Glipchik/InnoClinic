using Authorization.Application.Models;
using Authorization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddAccountMapping()
        {
            CreateMap<Account, AccountModel>();
        }
    }
}
