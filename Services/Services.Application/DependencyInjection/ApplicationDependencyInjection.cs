using Microsoft.Extensions.DependencyInjection;
using Services.Application.Services.Abstractions;
using Services.Application.Services;
using Services.Application.Mapper;

namespace Services.Application.Extensions
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApplicationMapping));

            services.AddScoped<IServiceCategoryManager, ServiceCategoryManager>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}
