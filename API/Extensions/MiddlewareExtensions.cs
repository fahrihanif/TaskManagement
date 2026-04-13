using API.Middlewares;

namespace API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();

    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        return app;
    }

}