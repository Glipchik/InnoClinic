using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Documents.Domain.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }
        IServiceCategoryRepository ServiceCategoryRepository { get; }
        IServiceRepository ServiceRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }
        IPatientRepository PatientRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);
    }
}
