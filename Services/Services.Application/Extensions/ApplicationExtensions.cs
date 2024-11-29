using Microsoft.Extensions.DependencyInjection;
using Services.Application.Services.Abstractions;
using Services.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Application.Mapper;

namespace Services.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
        {
            // Adding mapping profile
            services.AddAutoMapper(typeof(ApplicationMapping));

            // Adding services
            services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IServiceService, ServiceService>();

            return services;
        }
    }
}
