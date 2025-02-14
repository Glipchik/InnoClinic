using Profiles.Application.Models;
using Profiles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Services.Abstractions
{
    public interface IPatientService
    {
        Task Create(
            CreatePatientModel createPatientModel, 
            CreateAccountModel createAccountModel, 
            FileModel? fileModel, 
            CancellationToken cancellationToken);

        Task CreateFromAuthServer(
            CreatePatientModel createPatientModel,
            CreateAccountFromAuthServerModel createAccountFromAuthServerModel,
            FileModel? fileModel,
            CancellationToken cancellationToken);

        Task<PatientModel> Get(Guid id, CancellationToken cancellationToken);
        Task<PatientModel> GetByAccountId(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<PatientModel>> GetAll(CancellationToken cancellationToken);
        Task<PaginatedList<PatientModel>> GetAll(CancellationToken cancellationToken, int pageIndex = 1, int pageSize = 10);
        Task Update(
            UpdatePatientModel updatePatientModel,
            FileModel? fileModel,
            CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
