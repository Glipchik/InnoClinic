using Offices.Application.Models;
using Offices.Data.Entities;

namespace Offices.Application.Services.Abstractions
{
    public interface IOfficeService
    {
        Task Create(CreateOfficeModel createOfficeModel);
        Task<OfficeModel> Get(string id);
        Task<IEnumerable<OfficeModel>> GetAll();
        Task Update(UpdateOfficeModel updateOfficeModel);
        Task Delete(string id);
    }
}
