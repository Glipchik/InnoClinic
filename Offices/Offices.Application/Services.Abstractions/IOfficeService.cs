using Offices.Application.Models;
using Offices.Data.Entities;
using Offices.Domain.Models;

namespace Offices.Application.Services.Abstractions
{
    public interface IOfficeService
    {
        Task Create(CreateOfficeModel createOfficeModel, CancellationToken cancellationToken);
        Task<OfficeModel> Get(Guid id, CancellationToken cancellationToken);
        Task<PaginatedList<OfficeModel>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task Update(UpdateOfficeModel updateOfficeModel, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
