using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Repositories.Abstractions;
using Services.Domain.Entities;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entityToDelete);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
