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
        Task Create(CreateDoctorModel createDoctorModel, CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken);
        Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken);
        Task Update(UpdateDoctorModel updateDoctorModel, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
