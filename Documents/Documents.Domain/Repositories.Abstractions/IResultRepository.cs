using Documents.Domain.Entities;

namespace Documents.Domain.Repositories.Abstractions
{
    public interface IResultRepository
    {
        Task<IEnumerable<Result>> GetAllByPatientIdAsync(Guid patientId, CancellationToken cancellationToken);
        Task<IEnumerable<Result>> GetAllByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken);
    }
}
