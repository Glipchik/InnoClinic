using Microsoft.EntityFrameworkCore;
using Documents.Domain.Repositories.Abstractions;
using Documents.Domain.Entities;
using Documents.Infrastructure.Contexts;

namespace Documents.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual async Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await _context.Set<T>().FindAsync(id);
            ArgumentNullException.ThrowIfNull(entityToDelete);
            _context.Set<T>().Remove(entityToDelete);
            return entityToDelete;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public T Update(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
