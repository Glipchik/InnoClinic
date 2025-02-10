using Microsoft.EntityFrameworkCore;
using Documents.Domain.Entities;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Contexts;

namespace Documents.Infrastructure.Repositories
{
    public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        private readonly AppDbContext _context;

        public SpecializationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Specialization> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var specializationToDelete = await GetAsync(id, cancellationToken);
            ArgumentNullException.ThrowIfNull(specializationToDelete, nameof(specializationToDelete));
            specializationToDelete.IsActive = false;
            _context.Set<Specialization>().Update(specializationToDelete);
            return specializationToDelete;
        }
    }
}
