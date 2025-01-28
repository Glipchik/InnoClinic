using Documents.Application.Mapper;
using Documents.Application.Services;
using Documents.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApplicationMapping));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IResultService, ResultService>();

            return services;
        }
    }
}
