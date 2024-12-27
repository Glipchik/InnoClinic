using Services.Application.Mapper;
using Services.Application.Services.Abstractions;
using Services.Application.Services;
using Services.API.Mapper;
using Services.Application.Extensions;
using Services.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Services.API.Infrastructure;
using Services.API.Validators;
using FluentValidation;

namespace Services.API.DependencyInjection
{
    public static class ApiDependencyInjection
    {
        public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureDependencyInjection(configuration);

            services.AddProblemDetails();

            services.AddAutoMapper(typeof(ApiMapping));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddValidatorsFromAssemblyContaining<CreateDoctorDtoValidator>();

            services.AddApplicationDependencyInjection();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = configuration.GetSection("Authorization")["ServerUrl"];
                options.ClientId = configuration.GetSection("Authorization")["ClientId"];
                options.ClientSecret = configuration.GetSection("Authorization")["ClientSecret"];
                options.ResponseType = "code";
                options.Scope.Add("api.read");
                options.Scope.Add("api.write");
                options.Scope.Add("profile");
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.CallbackPath = "/signin-oidc";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey("role", "role", "string");
            });

            services.AddAuthorization();

            return services;
        }
    }
}
