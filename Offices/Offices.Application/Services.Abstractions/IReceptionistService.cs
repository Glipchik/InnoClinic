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
        Task Create(CreateReceptionistModel createReceptionistModel);
        Task<ReceptionistModel> Get(string id);
        Task<IEnumerable<ReceptionistModel>> GetAll();
        Task Update(UpdateReceptionistModel updateReceptionistModel);
        Task Delete(string id);
    }
}
