using Profiles.Application.Models;
using Profiles.Domain.Models;
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
            FileModel? fileModel, 
            CreateAccountModel createAccountModel, 
            CancellationToken cancellationToken);
        Task<DoctorModel> Get(Guid id, CancellationToken cancellationToken);
        Task<DoctorModel> GetByAccountId(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorModel>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<DoctorModel>> GetAll(DoctorQueryParametresModel doctorQueryParametresModel, CancellationToken cancellationToken);
        Task<PaginatedList<DoctorModel>> GetAll(DoctorQueryParametresModel doctorQueryParametresModel, CancellationToken cancellationToken,
            int pageIndex = 1, int pageSize = 10);
        Task Update(
            UpdateDoctorModel updateDoctorModel, 
            FileModel? fileModel,
            CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
