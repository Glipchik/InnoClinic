using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IAuthorizationService
    {
        Task<AuthorizationAccountModel> CreateAccount(CreateAccountAuthorizationServerModel createAccountAuthorizationServerModel, CancellationToken cancellationToken);
    }
}
