﻿using Documents.Domain.Entities;

namespace Documents.Domain.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken);
        T Update(T entity, CancellationToken cancellationToken);
        Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
    }
}
