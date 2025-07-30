using API.Middleware;

namespace API.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder
            .UseExceptionHandling()
            .UseRequestLogging();
    }
}