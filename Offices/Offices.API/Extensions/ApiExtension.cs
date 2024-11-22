using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Offices.API.MappingProfiles;
using Offices.Application.MappingProfiles;
using Offices.Application.Services;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Extensions;
using Offices.Data.Providers;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;

namespace Offices.API.Extensions
{
    public static class ApiExtension
    {
        public static IServiceCollection AddApiExtensions(this IServiceCollection services)
        {
            // Adding mapping profile
            services.AddAutoMapper(typeof(ApiMappingProfile));

            return services;
        }
    }
}