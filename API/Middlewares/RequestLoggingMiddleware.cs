using System.Diagnostics;

namespace API.Middlewares;

public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "[REQUEST] {Method} {Path} started at {Time}",
            context.Request.Method,
            context.Request.Path,
            DateTime.UtcNow.ToString("HH:mm:ss.fff"));

        // BEFORE
        await next(context);
        //AFTER
        
        stopwatch.Stop();

        _logger.LogInformation(
            "[RESPONSE] {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
