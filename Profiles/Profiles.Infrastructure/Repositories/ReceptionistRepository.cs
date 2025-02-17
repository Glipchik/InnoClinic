﻿using AutoMapper;
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
        private IAccountRepository _accountRepository;

        public ReceptionistRepository(AppDbContext context, IAccountRepository accountRepository) : base(context)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public override async Task<Receptionist> CreateAsync(Receptionist entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Receptionist>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public override async Task<Receptionist> UpdateAsync(Receptionist entity, CancellationToken cancellationToken)
        {
            _context.Set<Receptionist>().Update(entity);
            return entity;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var receptionist = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Receptionist with id {id} not found");

            _context.Set<Receptionist>().Remove(receptionist);

            await _accountRepository.DeleteAsync(receptionist.AccountId, id, cancellationToken);
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
