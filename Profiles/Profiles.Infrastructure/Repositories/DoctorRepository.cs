using AutoMapper;
using Events.Account;
using Events.Doctor;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Notification;
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
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public DoctorRepository(AppDbContext context, IPublishEndpoint publishEndpoint, IMapper mapper) : base(context)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public override async Task CreateAsync(Doctor entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Doctor>().AddAsync(entity, cancellationToken);

            await _publishEndpoint.Publish(_mapper.Map<DoctorCreated>(entity), cancellationToken);
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Doctor with id {id} not found");

            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);

            await _publishEndpoint.Publish(_mapper.Map<DoctorUpdated>(doctorToDelete), cancellationToken);
        }

        public override async Task UpdateAsync(Doctor entity, CancellationToken cancellationToken)
        {
            _context.Set<Doctor>().Update(entity);

            await _publishEndpoint.Publish(_mapper.Map<DoctorUpdated>(entity), cancellationToken);
        }

        public async override Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .Include(d => d.Account)
                .Include(d => d.Office)
                .ToListAsync(cancellationToken);
        }

        public async override Task<Doctor> GetAsync(Guid id, CancellationToken cancellationToken, bool isIncluded = true)
        {
            if (isIncluded)
            {
                return await _context.Set<Doctor>().AsNoTracking()
                    .Include(d => d.Specialization)
                    .Include(d => d.Account)
                    .Include(d => d.Office)
                    .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
            }

            return await _context.Set<Doctor>().AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<Doctor> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking()
                .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken: cancellationToken);
        }
    }
}
