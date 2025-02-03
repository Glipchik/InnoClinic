using Documents.Application.Models;

namespace Documents.Application.Services.Abstractions
{
    public interface IDoctorService
    {
        Task<DoctorModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    }
}
