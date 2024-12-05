using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;
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

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var doctorToDelete = await _context.Set<Doctor>().FindAsync(id);
            doctorToDelete.Status = Domain.Enums.DoctorStatus.Inactive;
            _context.Set<Doctor>().Update(doctorToDelete);
        }
    }
}
