using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.Services;
using Offices.Data.Extensions;

namespace Offices.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Adding services in container
            services.AddServices();

            // Adding repositories in container
            services.AddRepositories();

            return services;
        }
    }
}
