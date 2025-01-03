﻿using Authorization.Application.Mapper;
using Authorization.Application.Services;
using Authorization.Application.Services.Abstractions;
using Authorization.Data.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataDependencyInjection(configuration);

            services.AddAutoMapper(typeof(ApplicationMapping));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHashService, HashService>();

            return services;
        }
    }
}
