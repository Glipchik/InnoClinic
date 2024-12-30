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
using IdentityModel;

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
                options.ClientSecret = configuration.GetSection("AuthorizationSecrets")["ClientSecret"];
                options.ResponseType = "code";
                options.Scope.Add("profile");
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.Scope.Add("roles");
                options.CallbackPath = "/signin-oidc";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role);
                options.ClaimActions.MapJsonKey(JwtClaimTypes.Email, JwtClaimTypes.Email);
            });

            services.AddAuthorization();

            return services;
        }
    }
}
