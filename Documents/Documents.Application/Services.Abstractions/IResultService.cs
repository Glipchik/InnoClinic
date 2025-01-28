using Documents.Application.Models;
using Documents.Domain.Entities;

namespace Documents.Application.Services.Abstractions
{
    public interface IResultService
    {
        Task<ResultModel> Create(CreateResultModel createResultModel, CancellationToken cancellationToken);
        Task<ResultModel> Update(UpdateResultModel updateResultModel, CancellationToken cancellationToken);
        Task<ResultModel> Get(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ResultModel>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<ResultModel>> GetAllByPatientId(Guid patientId, CancellationToken cancellationToken);
        Task<IEnumerable<ResultModel>> GetAllByDoctorid(Guid doctorId, CancellationToken cancellationToken);
        Task<ResultModel> Delete(Guid id, CancellationToken cancellationToken);
        Task<ResultModel> SendResultByEmail(Guid id, string email, CancellationToken cancellationToken);
    }
}
