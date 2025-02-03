using Documents.Application.Models;

namespace Documents.Application.Services.Abstractions
{
    public interface IPatientService
    {
        Task<PatientModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    }
}
