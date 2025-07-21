using System.Diagnostics;

namespace TaskService.Api.Features.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        // Начало запроса
        _logger.LogInformation("Handling {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context); // Передаём управление дальше

        sw.Stop();

        // Завершение запроса
        _logger.LogInformation("Finished {Method} {Path} in {Elapsed} ms",
            context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds);
    }
}