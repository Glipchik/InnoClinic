using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
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

namespace Offices.API.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApiExtensions(this IServiceCollection services)
        {
            // Adding mapping profile
            services.AddAutoMapper(typeof(ApiMappingProfile));

            // Adding application extensions
            services.AddApplicationExtensions();

            // Global exception handler
            services.AddExceptionHandler<GlobalExceptionHandler>();

            // Adding problem details
            services.AddProblemDetails();

            // Adding validators (it will register all the validators in the same assembly in which CreateOfficeDtoValidator is defined)
            services.AddValidatorsFromAssemblyContaining<CreateOfficeDtoValidator>();

            return services;
        }

        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState, ILogger logger)
        {
            foreach (var error in result.Errors)
            {
                logger.LogInformation("Validation error with property {propertyName }: {errorMessage}", error.PropertyName, error.ErrorMessage);
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}