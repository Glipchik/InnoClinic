using Authorization.Application.Mapper;
using Authorization.Application.Services;
using Authorization.Application.Services.Abstractions;
using Authorization.Data.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataDependencyInjection(configuration);

            services.AddAutoMapper(typeof(ApplicationMapping));

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddMemoryCache();
            services.AddSingleton<IEmailTokenStoreService, EmailTokenStoreService>();

            return services;
        }
    }
}