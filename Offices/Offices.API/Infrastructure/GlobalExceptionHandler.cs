using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Offices.Domain.Exceptions;

namespace Offices.API.Infrastructure
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
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

            if (exception is BadRequestException badRequestException)
            {
                if (exception is ValidationException validationException)
                {
                    var errorMessages = validationException.Errors
                        .SelectMany(kvp => kvp.Value.Select(err => $"{kvp.Key}: {err}"))
                        .ToList();
                    problemDetails = new ValidationProblemDetails(validationException.Errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Error",
                        Detail = string.Join(", ", errorMessages)
                    };
                }
                else
                {
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
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
