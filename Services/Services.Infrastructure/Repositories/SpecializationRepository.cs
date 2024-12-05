using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly AppDbContext _context;

        public SpecializationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var specializationToDelete = await _context.Set<Specialization>().FindAsync(id);
            specializationToDelete.IsActive = false;
            _context.Set<Specialization>().Update(specializationToDelete);
        }

        public Task<(IEnumerable<Doctor> doctorsRelatedToSpecialization, Specialization? specialization)> GetSpecializationAndRelatedActiveDoctors(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
