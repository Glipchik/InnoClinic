﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
using Services.Domain.Exceptions;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsWithSpecializationAsync(Guid specializationId, CancellationToken cancellationToken)
        {
            return await _context.Set<Doctor>().AsNoTracking().Where(d => d.SpecializationId == specializationId && d.Status != Domain.Enums.DoctorStatus.Inactive).ToListAsync(cancellationToken);
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await _context.Set<Doctor>().FindAsync(id, cancellationToken) ?? throw new NotFoundException($"Doctor with id: {id} not found. Can't delete.");
            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);
        }
    }
}
