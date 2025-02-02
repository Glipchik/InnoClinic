using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Core.Servers;
using Offices.API.DTOs;
using Offices.API.Infrastructure;
using Offices.API.MappingProfiles;
using Offices.API.Validators;
using Offices.Application.Extensions;
using Offices.Application.MappingProfiles;
using Offices.Application.Services;
using Offices.Application.Services.Abstractions;
using Offices.Data.Entities;
using Offices.Data.Extensions;
using Offices.Data.Providers;
using Offices.Data.Repositories;
using Offices.Data.Repositories.Abstractions;
using Offices.MessageBroking.DependencyInjection;

namespace Offices.API.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApiExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            // Adding mapping profile
            services.AddAutoMapper(typeof(ApiMappingProfile));

            // Adding application extensions
            services.AddApplicationExtensions();

            services.AddMessageBrokingDependencyInjection();

            // Global exception handler
            services.AddExceptionHandler<GlobalExceptionHandler>();

            // Adding problem details
            services.AddProblemDetails();

            // Adding validators (it will register all the validators in the same assembly in which CreateOfficeDtoValidator is defined)
            services.AddValidatorsFromAssemblyContaining<CreateOfficeDtoValidator>();

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