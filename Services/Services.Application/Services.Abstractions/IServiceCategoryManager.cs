using Services.Application.Models;
using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IServiceCategoryManager : IGenericService<CreateServiceCategoryModel, ServiceCategoryModel, UpdateServiceCategoryModel>
    {
        Task<PaginatedList<ServiceCategoryModel>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
