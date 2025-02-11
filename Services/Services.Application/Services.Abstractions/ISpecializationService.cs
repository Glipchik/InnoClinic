using Services.Application.Models;
using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface ISpecializationService : IGenericService<CreateSpecializationModel, SpecializationModel, UpdateSpecializationModel>
    {
        Task<PaginatedList<SpecializationModel>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
