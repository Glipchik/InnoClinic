using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.Services;
using Offices.Data.Repositories;

namespace Offices.Application.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Adding services in container
            services.AddScoped<IOfficeService, OfficeService>();

            return services;
        }
    }
}
