using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Account> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Set<Account>().AsNoTracking()
               .FirstOrDefaultAsync(a => a.Email.Equals(email), cancellationToken: cancellationToken);
        }
    }
}
