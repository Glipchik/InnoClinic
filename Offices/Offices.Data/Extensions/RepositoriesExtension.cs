using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Offices.Data.Entities;
using Offices.Data.Providers;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;

namespace Offices.Data.Extensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Adding database context in container
            services.AddScoped<MongoDbContext>();

            // Adding repositories in container
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IReceptionistRepository, ReceptionistRepository>();

            return services;
        }
    }
}