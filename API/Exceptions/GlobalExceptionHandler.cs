using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
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
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            ValidationException         => (StatusCodes.Status422UnprocessableEntity, "Validation Failed"),
            ArgumentException           => (StatusCodes.Status400BadRequest,           "Bad Request"),
            KeyNotFoundException        => (StatusCodes.Status404NotFound,             "Not Found"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,         "Unauthorized"),
            _                           => (StatusCodes.Status500InternalServerError,  "Internal Server Error")
        };

        var problemDetails = new ProblemDetails
        {
            Type     = $"https://httpstatuses.io/{statusCode}",
            Title    = title,
            Status   = statusCode,
            Detail   = exception.Message,
            Instance = httpContext.Request.Path
        };
        
        if (exception is ValidationException ve)
        {
            problemDetails.Extensions["errors"] = ve.Errors;
        }

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response
                         .WriteAsJsonAsync(problemDetails, cancellationToken);

        // Return true = exception is handled, pipeline stops here
        return true;
    }
}