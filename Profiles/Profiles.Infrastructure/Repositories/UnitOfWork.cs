using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Profiles.Domain.Repositories.Abstractions;
using Profiles.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;


        private IDoctorRepository _doctorRepository;
        public IDoctorRepository DoctorRepository => _doctorRepository;


        private IAccountRepository _accountRepository;
        public IAccountRepository AccountRepository => _accountRepository;


        private IPatientRepository _patientRepository;
        public IPatientRepository PatientRepository => _patientRepository;


        private IReceptionistRepository _receptionistRepository;
        public IReceptionistRepository ReceptionistRepository => _receptionistRepository;


        private IOfficeRepository _officeRepository;
        public IOfficeRepository OfficeRepository => _officeRepository;


        private ISpecializationRepository _specializationRepository;
        public ISpecializationRepository SpecializationRepository => _specializationRepository;


        public UnitOfWork(AppDbContext context,
            IDoctorRepository doctorRepository,
            IReceptionistRepository receptionistRepository,
            IOfficeRepository officeRepository,
            IPatientRepository patientRepository,
            IAccountRepository accountRepository,
            ISpecializationRepository specializationRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _doctorRepository = doctorRepository;
            _receptionistRepository = receptionistRepository;
            _officeRepository = officeRepository;
            _patientRepository = patientRepository;
            _accountRepository = accountRepository;
            _specializationRepository = specializationRepository;
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
