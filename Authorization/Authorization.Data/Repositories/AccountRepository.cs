using Authorization.Data.Providers;
using Authorization.Data.Repositories.Abstractions;
using Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Account> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(account => account.Email == email)
                .SingleOrDefaultAsync();
        }
    }
}
