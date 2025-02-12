using Services.Domain.Entities;
using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetAllAsync(Guid? seviceCategoryId, Guid? specializationId, bool? isActive, CancellationToken cancellationToken);
        Task<PaginatedList<Service>> GetAllAsync(Guid? serviceCategoryId, Guid? specializationId, bool? isActive, int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
