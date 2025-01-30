using AutoMapper;
using Events.Patient;
using Events.Receptionist;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;

namespace Profiles.Infrastructure.Repositories
{
    public class ReceptionistRepository : GenericRepository<Receptionist>, IReceptionistRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private IAccountRepository _accountRepository;

        public ReceptionistRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper, IAccountRepository accountRepository) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        public override async Task CreateAsync(Receptionist entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Receptionist>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<ReceptionistCreated>(entity), cancellationToken);
        }

        public override async Task UpdateAsync(Receptionist entity, CancellationToken cancellationToken)
        {
            _context.Set<Receptionist>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<ReceptionistUpdated>(entity), cancellationToken);
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var receptionist = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Receptionist with id {id} not found");

            _context.Set<Receptionist>().Remove(receptionist);

            await _accountRepository.DeleteAsync(receptionist.AccountId, id, cancellationToken);

            await _publishEndpoint.Publish(new ReceptionistDeleted() { Id = id }, cancellationToken);
        }

        public async override Task<IEnumerable<Receptionist>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Receptionist>().AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Office)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Receptionist> GetAsync(Guid id, CancellationToken cancellationToken, bool isIncluded = true)
        {
            if (isIncluded)
            {
                return await _context.Set<Receptionist>().AsNoTracking()
                    .Include(r => r.Account)
                    .Include(r => r.Office)
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
            }

            return await _context.Set<Receptionist>().AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<Receptionist> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Receptionist>().AsNoTracking()
                .FirstOrDefaultAsync(r => r.AccountId == accountId, cancellationToken: cancellationToken);
        }
    }
}
