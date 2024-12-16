using Authorization.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services.Abstractions
{
    public interface IAccountService
    {
        Task<string> Register(RegisterAccountModel registerAccountModel, CancellationToken cancellationToken);
        Task<string> Login(LoginAccountModel loginAccountModel, CancellationToken cancellationToken);
    }
}
