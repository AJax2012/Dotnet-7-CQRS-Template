using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;

using SourceName.Api.Endpoints.Common.Extensions;

namespace SourceName.Api.Endpoints.Common.Filters;

/// <summary>
/// Validates a request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public class RequestValidationFilter<TRequest> : IEndpointFilter
{
	private readonly ILogger _logger;
	private readonly IValidator<TRequest>? _validator;

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestValidationFilter{TRequest}"/> class.
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="validator"></param>
	public RequestValidationFilter(ILogger logger, IValidator<TRequest>? validator)
	{
		_logger = logger;
		_validator = validator;
	}

	/// <inheritdoc />
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var requestName = typeof(TRequest).FullName;

		if (_validator is null)
		{
			_logger.Information("{RequestName}: validation skipped.", requestName);
			return await next(context);
		}

		var request = context.Arguments.OfType<TRequest>().First();
		var validationResult = await _validator.ValidateAsync(request);
		
		if (!validationResult.IsValid)
		{
			_logger.Error("{RequestName}: validation failed. {ValidationErrors}", requestName, validationResult.Errors);
			return validationResult.Errors
				.Select(x => Error.Validation(x.ErrorCode, x.ErrorMessage))
				.ToImmutableList()
				.ToProblemDetailsResult();
		}
		
		_logger.Information("{RequestName}: validation succeeded.", requestName);
		return await next(context);
	}
}