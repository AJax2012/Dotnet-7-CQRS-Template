using System.Diagnostics;
using MediatR;
using Serilog;

namespace SourceName.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.Information("{Request} is starting", request.GetType().Name);

        var timer = Stopwatch.StartNew();
        var response = await next();
        timer.Stop();
        
        _logger.Information("{Request} has finished in {Time} ms", request.GetType().Name, timer.ElapsedMilliseconds);
        return response;
    }
}