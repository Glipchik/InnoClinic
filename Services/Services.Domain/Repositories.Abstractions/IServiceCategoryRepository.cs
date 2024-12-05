using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IServiceCategoryRepository : IGenericRepository<ServiceCategory>
    {
        Task<(IEnumerable<Service> servicesRelatedToCategory, ServiceCategory? serviceCategory)> GetServiceCategoryAndRelatedActiveServices(Guid id, CancellationToken cancellationToken);
    }
}
