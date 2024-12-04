using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Domain.Entities;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }
        IServiceCategoryRepository ServiceCategoryRepository { get; }
        IServiceRepository ServiceRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
