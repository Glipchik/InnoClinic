using Appointments.Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Appointments.Infrastructure.Contexts;

namespace Appointments.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        private IDoctorRepository _doctorRepository;
        public IDoctorRepository DoctorRepository => _doctorRepository;

        private IServiceCategoryRepository _serviceCategoryRepository;
        public IServiceCategoryRepository ServiceCategoryRepository => _serviceCategoryRepository;

        private IServiceRepository _serviceRepository;
        public IServiceRepository ServiceRepository => _serviceRepository;

        private ISpecializationRepository _specializationRepository;
        public ISpecializationRepository SpecializationRepository => _specializationRepository;

        private IPatientRepository _patientRepository;
        public IPatientRepository PatientRepository => _patientRepository;

        private IAppointmentRepository _appointmentRepository;
        public IAppointmentRepository AppointmentRepository => _appointmentRepository;

        public UnitOfWork(
            AppDbContext context,
            IDoctorRepository doctorRepository,
            IServiceCategoryRepository serviceCategoryRepository,
            IServiceRepository serviceRepository,
            ISpecializationRepository specializationRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository)
        {
            _context = context;
            _doctorRepository = doctorRepository;
            _serviceCategoryRepository = serviceCategoryRepository;
            _serviceRepository = serviceRepository;
            _specializationRepository = specializationRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction BeginTransaction(
            System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = _context.Database.BeginTransaction(isolationLevel);

            return _currentTransaction;
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }
    }
}
