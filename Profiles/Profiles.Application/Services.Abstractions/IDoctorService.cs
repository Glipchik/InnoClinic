using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IDoctorService
    {
        Task Create(
            CreateDoctorModel createDoctorModel, 
            FileModel fileModel, 
            CreateAccountModel createAccountModel, 
            CancellationToken cancellationToken);
        Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken);
        Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
