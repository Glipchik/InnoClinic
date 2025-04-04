﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Repositories.Abstractions;
using Services.Domain.Entities;
using Services.Infrastructure.Contexts;
using Services.Domain.Models;

namespace Services.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            await _context.Set<T>().AddAsync(entity, cancellationToken);

            return entity;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityToDelete = await GetAsync(id, cancellationToken)
                ?? throw new ArgumentNullException($"Entity with id {id} to delete is null.");

            _context.Set<T>().Remove(entityToDelete);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<T>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var paginatedEntities = await _context.Set<T>()
                .AsNoTracking()
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var count = await _context.Set<T>().CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<T>(paginatedEntities, pageIndex, totalPages);
        }

        public virtual async Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
