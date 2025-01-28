using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IAuthorizationServerManager
    {
        Task<AuthorizationAccountModel> CreateAccount(CreateAccountAuthorizationServerModel createAccountAuthorizationServerModel, CancellationToken cancellationToken);
    }
}
