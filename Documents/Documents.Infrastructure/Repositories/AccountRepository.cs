using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
