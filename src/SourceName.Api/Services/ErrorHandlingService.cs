using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using SourceName.Application.Common.Errors;

namespace SourceName.Api.Services;

public static class ErrorHandlingService
{
    internal static ActionResult HandleError(Error error) =>
        error == Errors.User.NotFound ? new UnauthorizedResult() :
        error == Errors.Entity.NotFound ? new NotFoundObjectResult(new { error = error.Description }) :
        new StatusCodeResult(StatusCodes.Status500InternalServerError);
}