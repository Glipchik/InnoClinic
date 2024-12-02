using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        // Репозитории
        private IDoctorRepository _doctorRepository;
        private IServiceCategoryRepository _serviceCategoryRepository;
        private IServiceRepository _serviceRepository;
        private ISpecializationRepository _specializationRepository;

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

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        // Методы для получения репозиториев
        public IDoctorRepository GetDoctorRepository() => _doctorRepository;

        public IServiceCategoryRepository GetServiceCategoryRepository() => _serviceCategoryRepository;

        public IServiceRepository GetServiceRepository() => _serviceRepository;

        public ISpecializationRepository GetSpecializationRepository() => _specializationRepository;
    }
}
