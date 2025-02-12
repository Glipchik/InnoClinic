using Services.Application.Models;
using Services.Domain.Entities;
using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IServiceManager : IGenericService<CreateServiceModel, ServiceModel, UpdateServiceModel>
    {
        Task<IEnumerable<ServiceModel>> GetAll(ServiceQueryParametresModel serviceQueryParametresModel, CancellationToken cancellationToken);
        Task<PaginatedList<ServiceModel>> GetAll(ServiceQueryParametresModel serviceQueryParametresModel, int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
