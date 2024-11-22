using Offices.Application.Models;
using Offices.Data.Entities;

namespace Offices.Application.Services.Abstractions
{
    public interface IOfficeService
    {
        Task Create(CreateOfficeModel createOfficeModel, CancellationToken cancellationToken);
        Task<OfficeModel> Get(string id, CancellationToken cancellationToken);
        Task<IEnumerable<OfficeModel>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateOfficeModel updateOfficeModel, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}
