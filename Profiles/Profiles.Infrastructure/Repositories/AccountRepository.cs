using AutoMapper;
using Events.Account;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Notification;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;

namespace Profiles.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public AccountRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task CreateAsync(Account entity, Guid authorId, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = authorId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = authorId;

            await _context.Set<Account>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<AccountCreated>(entity), cancellationToken);
        }

        public async Task DeleteAsync(Guid id, Guid authorId, CancellationToken cancellationToken)
        {
            var entityToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Account with id {id} not found");

            _context.Set<Account>().Remove(entityToDelete);

            await _publishEndpoint.Publish(new AccountDeleted() { Id = id }, cancellationToken);
        }

        public async Task<Account> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Set<Account>().AsNoTracking()
               .FirstOrDefaultAsync(a => a.Email.Equals(email), cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Account>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Account> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Account>().FindAsync(id);
        }

        public async Task UpdateAsync(Account entity, Guid authorId, CancellationToken cancellationToken)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = authorId;

            _context.Set<Account>().Update(entity);
        }
    }
}
