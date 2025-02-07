using Profiles.API.Infrastructure;
using System;
using Profiles.Application.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using Profiles.API.Mapper;
using FluentValidation;
using Profiles.API.Validators;
using System.Reflection;
using Profiles.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Duende.IdentityServer.Models;
using Services.Consumers.Consumers.DependencyInjection;

namespace Profiles.API.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDependencyInjection();
        
        services.AddInfrastructureDependencyInjection(configuration);

        services.AddMessageBrokingDependencyInjection();

        services.AddProblemDetails();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddAutoMapper(typeof(ApiMapping));

        services.AddValidatorsFromAssemblyContaining<CreateDoctorDtoValidator>();

        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Profiles API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Bearer";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies", options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = configuration.GetSection("Authorization")["ServerUrl"];
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        })
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireCreatePatientProfileScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "create_patient_profile");
                policy.AuthenticationSchemes.Add("Bearer");
            });
        });

        services.AddHttpClient();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost3000",
                builder => builder.WithOrigins("http://localhost:3000")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials());
        });

        return services;
    }
}
