using Services.Application.Mapper;
using Services.Application.Services.Abstractions;
using Services.Application.Services;
using Services.API.Mapper;
using Services.Application.Extensions;
using Services.Infrastructure.Extensions;
using Services.API.Infrastructure;
using Services.API.Validators;
using FluentValidation;

namespace Services.API.DependencyInjection
{
    public static class ApiDependencyInjection
    {
        public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProblemDetails();

            services.AddAutoMapper(typeof(ApiMapping));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddValidatorsFromAssemblyContaining<CreateDoctorDtoValidator>();

            services.AddApplicationDependencyInjection();

            services.AddInfrastructureDependencyInjection(configuration);

            return services;
        }
    }
}
