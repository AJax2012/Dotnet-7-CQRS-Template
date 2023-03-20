using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

using ILogger = Serilog.ILogger;

namespace SourceName.Api.Filters;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger logger, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedAccessException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnknownException(context, ex);
        }
    }

    private async Task HandleUnauthorizedAccessException(HttpContext context, Exception exception)
    {
        _logger.Error(exception, "User denied access");
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "An error occurred while processing your request.",
            Type = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1",
            Detail = "You have been denied access.",
        }));
    }

    private async Task HandleUnknownException(HttpContext context, Exception exception)
    {
        _logger.Error(exception, "Unknown exception");
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = _hostEnvironment.IsDevelopment()
                ? exception.Message
                : "An unknown error occurred. If the issue persists, please contact the developer.",
        }));
    }
}