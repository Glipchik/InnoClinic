using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IReceptionistService
    {
        Task Create(CreateReceptionistModel createReceptionistModel, CreateAccountModel createAccountModel, CancellationToken cancellationToken);
        Task<ReceptionistModel> Get(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateReceptionistModel updateReceptionistModel, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
