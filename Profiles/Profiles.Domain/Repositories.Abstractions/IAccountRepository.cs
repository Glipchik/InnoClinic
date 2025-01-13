using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Domain.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken);
        Task<Account> GetAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Account entity, Guid authorId, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, Guid authorId, CancellationToken cancellationToken);
        Task CreateAsync(Account entity, Guid authorId, CancellationToken cancellationToken);
        Task<Account> FindByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
