using Microsoft.Extensions.DependencyInjection;
using Documents.Domain.Repositories.Abstractions;
using Documents.Infrastructure.Repositories;
using Documents.Infrastructure.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
