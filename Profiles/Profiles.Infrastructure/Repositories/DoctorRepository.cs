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

        public DoctorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Doctor> CreateAsync(Doctor entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<Doctor>().AddAsync(entity, cancellationToken);

            return entity;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Doctor with id {id} not found");

            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);
        }

        public override async Task<Doctor> UpdateAsync(Doctor entity, CancellationToken cancellationToken)
        {
            _context.Set<Doctor>().Update(entity);
            return entity;
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

        public async Task<IEnumerable<Doctor>> GetAllAsync(Guid? specializationId, Domain.Enums.DoctorStatus? status, CancellationToken cancellationToken)
        {
            var query = _context.Set<Doctor>().AsNoTracking()
                .Include(d => d.Specialization)
                .Include(d => d.Account)
                .Include(d => d.Office)
                .AsQueryable();

            if (specializationId.HasValue)
            {
                query = query.Where(d => d.SpecializationId == specializationId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(d => d.Status.Equals(status.Value));
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
