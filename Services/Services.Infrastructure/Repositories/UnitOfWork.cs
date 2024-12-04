using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
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


        public UnitOfWork(AppDbContext context,
            IDoctorRepository doctorRepository,
            IServiceCategoryRepository serviceCategoryRepository,
            IServiceRepository serviceRepository,
            ISpecializationRepository specializationRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _doctorRepository = doctorRepository;
            _serviceCategoryRepository = serviceCategoryRepository;
            _serviceRepository = serviceRepository;
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

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to commit.");
            }

            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("There is no active transaction to roll back.");
            }

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }
    }
}
