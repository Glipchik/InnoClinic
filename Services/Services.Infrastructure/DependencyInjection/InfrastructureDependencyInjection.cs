using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Repositories;
using Services.Infrastructure.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Services.Infrastructure.Extensions
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
