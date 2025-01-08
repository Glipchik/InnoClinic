using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> FindByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
