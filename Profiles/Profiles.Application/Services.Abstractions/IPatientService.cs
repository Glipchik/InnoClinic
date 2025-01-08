using Profiles.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IPatientService
    {
        Task Create(CreatePatientModel createPatientModel, CreateAccountModel createAccountModel, Guid authorId, CancellationToken cancellationToken);
        Task<PatientModel> Get(Guid id, CancellationToken cancellationToken);
        Task Update(UpdatePatientModel updatePatientModel,CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
