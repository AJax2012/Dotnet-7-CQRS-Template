using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace SourceName.Api.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger _logger;
    private readonly Dictionary<Type, Action<ExceptionContext>> _handlers;

    public ApiExceptionFilter(ILogger logger)
    {
        _logger = logger;
        _handlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_handlers.ContainsKey(type))
        {
            _handlers[type].Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        _logger.Error(context.Exception, "User denied access");
        context.Result = new UnauthorizedResult();
        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        _logger.Error(context.Exception, "Unknown exception");
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = context.Exception.Message,
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }
}