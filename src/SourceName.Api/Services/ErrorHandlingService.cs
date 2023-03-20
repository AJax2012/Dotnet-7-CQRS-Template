using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using SourceName.Application.Common.Errors;

namespace SourceName.Api.Services;

public static class ErrorHandlingService
{
    private const string Title = "An error occurred while processing your request.";
    private const string BaseRfcUrl = "https://www.rfc-editor.org/rfc";

    internal static IResult HandleErrors(Error error) =>
        error == Errors.Entity.Exists ? Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = Title,
            Type = Path.Combine(BaseRfcUrl, "rfc7231#section-6.5.1"),
            Detail = error.Description,
        }) :
        error == Errors.User.NotFound || error == Errors.User.Unauthorized ? Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = Title,
            Type = Path.Combine(BaseRfcUrl, "fc7235#section-3.1"),
            Detail = error.Description,
        }) :
        error == Errors.Entity.NotFound ? Results.Problem(new ProblemDetails
        {
                Status = StatusCodes.Status404NotFound,
                Title = Title,
                Type = Path.Combine(BaseRfcUrl, "rfc7231#section-6.5.4"),
                Detail = error.Description,
        }) :
        Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = Title,
            Type = Path.Combine(BaseRfcUrl, "rfc7231#section-6.6.1"),
            Detail = error.Description,
        });
}