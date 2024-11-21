using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Application.Models;

namespace Offices.Application.Services.Abstractions
{
    public interface IReceptionistService
    {
        Task Create(CreateReceptionistModel createReceptionistModel, CancellationToken cancellationToken);
        Task<ReceptionistModel> Get(string id, CancellationToken cancellationToken);
        Task<IEnumerable<ReceptionistModel>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateReceptionistModel updateReceptionistModel, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}
