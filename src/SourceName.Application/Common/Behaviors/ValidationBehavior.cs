using System.Net;
using MediatR;
using Serilog;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;

namespace SourceName.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
    where TResponse : CqrsResponse, new()
{
    private readonly ILogger _logger;
    private readonly IValidationHandler<TRequest> _validationHandler;

    public ValidationBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public ValidationBehavior(ILogger logger, IValidationHandler<TRequest> validationHandler)
    {
        _logger = logger;
        _validationHandler = validationHandler;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = request.GetType().Name;

        if (_validationHandler is null)
        {
            _logger.Information("{Request} does not have a validation handler configured", requestName);
            return await next();
        }

        var result = await _validationHandler.Validate(request);

        if (!result.IsSuccessful)
        {
            _logger.Warning("Validation failed for {Request}. Error: {Error}", requestName, result.Error);
            return new TResponse
            {
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessage = result.Error,
            };
        }
        
        _logger.Information("Validation successful for {Request}", requestName);
        return await next();
    }
}