using System.Collections.Generic;
using System.Linq;

using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleProject.Api.Endpoints.Common.Extensions;

/// <summary>
/// Creates a factory for mapping 0 or more <see cref="Error"/>s into a <see cref="ProblemDetails"/> result.
/// </summary>
public static class ProblemDetailsFactory
{
	/// <summary>
	/// Crates a <see cref="ProblemDetails"/> result from 0 or more <see cref="Error"/>s.
	/// </summary>
	/// <param name="errors">List of <see cref="Error"/>.</param>
	/// <returns><see cref="IResult"/> of <see cref="ProblemDetails"/>.</returns>
	public static IResult ToProblemDetailsResult(this IList<Error> errors)
	{
		if (errors.Count is 0)
		{
			return Results.Problem();
		}

		return errors.All(error => error.Type == ErrorType.Validation) ?
			ValidationProblem(errors) : Problem(errors[0]);
	}

	private static IResult Problem(Error error)
	{
		var statusCode = error.Type switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,
		};

		return Results.Problem(statusCode: statusCode, detail: error.Description);
	}

	private static IResult ValidationProblem(IEnumerable<Error> errors)
	{
		IDictionary<string, string[]> errorDictionary =
			errors.ToDictionary(error => error.Code, error => new[] { error.Description });

		return Results.ValidationProblem(errorDictionary);
	}
}
