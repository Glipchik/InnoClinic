using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Application.Models;

namespace Offices.Application.Services.Abstractions
{
    public interface IDoctorService
    {
        Task Create(CreateDoctorModel createDoctorModel, CancellationToken cancellationToken);
        Task<DoctorModel> Get(string id, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}