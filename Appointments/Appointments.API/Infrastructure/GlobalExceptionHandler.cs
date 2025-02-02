using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Appointments.Application.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Appointments.API.Infrastructure
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            ProblemDetails problemDetails;

            if (exception is FluentValidation.ValidationException ex)
            {
                var errorMessages = ex.Errors
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
                    .ToList();

                problemDetails = new ValidationProblemDetails(
                    ex.Errors.GroupBy(e => e.PropertyName)
                             .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Detail = string.Join(", ", errorMessages)
                };
            }
            else if (exception is BadRequestException badRequestException)
            {   
                if (exception is NotFoundException notFoundException)
                {
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
                        Detail = notFoundException.Message
                    };
                }
                else if (exception is ForbiddenException forbiddenException)
                {
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "Forbidden",
                        Detail = forbiddenException.Message
                    };
                } 
                else
                {
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Bad Request",
                        Detail = badRequestException.Message
                    };
                }
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = "An unexpected error occurred. Please try again later."
                };
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
