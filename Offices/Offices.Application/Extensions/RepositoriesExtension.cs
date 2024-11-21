using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.MappingProfiles;
using Offices.Application.Services;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Extensions;
using Offices.Data.Providers;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Application.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Adding mapping profile
            services.AddAutoMapper(typeof(ApplicationMappingProfile));

            // Adding services
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IReceptionistService, ReceptionistService>();

            // Adding repositories in container
            services.AddRepositories();

            return services;
        }
    }
}