using AutoMapper;
using Events.Doctor;
using Events.Patient;
using MassTransit;
using MassTransit.Transports;
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
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public PatientRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper, IAccountRepository accountRepository) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var patient = await  GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Patient with id {id} not found");

            _context.Set<Patient>().Remove(patient);
            
            await _accountRepository.DeleteAsync(patient.AccountId, id, cancellationToken);

            await _publishEndpoint.Publish(new PatientDeleted() { Id = id }, cancellationToken);
        }

        public override async Task CreateAsync(Patient entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Patient>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<PatientCreated>(entity), cancellationToken);
        }

        public override async Task UpdateAsync(Patient entity, CancellationToken cancellationToken)
        {
            _context.Set<Patient>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<PatientUpdated>(entity), cancellationToken);
        }

        public async override Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .Include(p => p.Account)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Patient> GetAsync(Guid id, CancellationToken cancellationToken, bool isIncluded = true)
        {
            if (isIncluded)
            {
                return await _context.Set<Patient>().AsNoTracking()
                    .Include(p => p.Account)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
            }

            return await _context.Set<Patient>().AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<Patient> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Patient>().AsNoTracking()
                .FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken: cancellationToken);
        }
    }
}
