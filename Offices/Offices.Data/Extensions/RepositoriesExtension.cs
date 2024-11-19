using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Offices.Data.Entities;
using Offices.Data.Repositories;

namespace Offices.Data.Extensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Adding repositories in container
            services.AddScoped<IGenericRepository<BaseEntity>, GenericRepository<BaseEntity>>();

            return services;
        }
    }
}