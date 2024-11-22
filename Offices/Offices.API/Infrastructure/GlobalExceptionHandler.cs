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
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            ProblemDetails problemDetails;

            if (exception is ValidationException badRequestException)
            {
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = badRequestException.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                // Обработка всех других исключений
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server error",
                    Detail = "An unexpected error occurred. Please try again later."
                };
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error",
                Detail = "An unexpected error occurred. Please try again later."
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
