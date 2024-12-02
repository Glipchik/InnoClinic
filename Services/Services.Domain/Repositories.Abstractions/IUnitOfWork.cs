using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain.Entities;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IDoctorRepository GetDoctorRepository();
        IServiceCategoryRepository GetServiceCategoryRepository();
        IServiceRepository GetServiceRepository();
        ISpecializationRepository GetSpecializationRepository();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
