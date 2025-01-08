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
        Task Create(CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken);
        Task<AccountModel> FindByEmail(string email, CancellationToken cancellationToken);
    }
}
