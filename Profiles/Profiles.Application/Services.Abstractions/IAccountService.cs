using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IAccountService
    {
        Task<AccountModel> Get(Guid id, CancellationToken cancellationToken);
        Task<AccountModel> Create(CreateAccountModel createAccountModel, CancellationToken cancellationToken);
        Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken);
    }
}
