using Services.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IGenericService<CreateModel, Model, UpdateModel>
    where Model : BaseModel
    where UpdateModel : BaseModel
    {
        Task Create(CreateModel createModel, CancellationToken cancellationToken);
        Task<Model> Get(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Model>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateModel updateModel, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
