using Microsoft.AspNetCore.Authentication;
using FluentValidation;
using IdentityModel;
using System.Reflection;
using Appointments.Infrastructure.DependencyInjection;
using Appointments.Application.DependencyInjection;
using Appointments.API.Infrastructure;
using Appointments.API.DTOs;
using Appointments.API.Validators;
using Appointments.API.Mapper;
using Microsoft.IdentityModel.Tokens;
using Appointments.MessageBroking.DependencyInjection;

namespace Appointments.API.DependencyInjection
{
    public static class ApiDependencyInjection
    {
        public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureDependencyInjection(configuration);

            services.AddMessageBrokingDependencyInjection();

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddProblemDetails();

            services.AddAutoMapper(typeof(ApiMapping));

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddValidatorsFromAssemblyContaining<CreateAppointmentDtoValidator>();

            services.AddApplicationDependencyInjection();

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

            services.AddAuthorization();

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

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
}
