using Authorization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Data.Repositories.Abstractions
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Account> GetByExternalProviderAsync(string provider, Guid accountId, CancellationToken cancellationToken);
    }
}
