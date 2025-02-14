using Profiles.Application.Models;
using Profiles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IReceptionistService
    {
        Task Create(
            CreateReceptionistModel createReceptionistModel, 
            CreateAccountModel createAccountModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken);
        Task<ReceptionistModel> Get(Guid id, CancellationToken cancellationToken);
        Task<ReceptionistModel> GetByAccountId(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken, ReceptionistQueryParametresModel receptionistQueryParametresModel);
        Task<PaginatedList<ReceptionistModel>> GetAll(CancellationToken cancellationToken, ReceptionistQueryParametresModel receptionistQueryParametresModel,
            int pageIndex = 1, int pageSize = 10);
        Task Update(
            UpdateReceptionistModel updateReceptionistModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
