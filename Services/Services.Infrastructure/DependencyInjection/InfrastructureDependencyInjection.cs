using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Mapper;
using Services.Application.Services.Abstractions;
using Services.Application.Services;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Repositories;

namespace Services.Infrastructure.Extensions
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencyInjection(this IServiceCollection services)
        {
            // Adding repositories
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();

            // Adding unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
