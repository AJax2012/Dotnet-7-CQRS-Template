using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using ExampleProject.Api.Endpoints.Common.Contracts;
using ExampleProject.Api.Endpoints.Common.Extensions;
using ExampleProject.Application.Profiles.Commands.Delete;

namespace ExampleProject.Api.Endpoints.Profiles;

/// <summary>
/// Endpoint handler for deleting a Profile.
/// </summary>
public class DeleteProfile : IEndpoint
{
    /// <inheritdoc />
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app) => app
        .MapDelete("{id:guid}", HandleAsync)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound, "application/json")
        .WithName("DeleteProfile");

    /// <summary>
    /// API request for deleting a Profile.
    /// </summary>
    /// <param name="id">Unique id for Profile.</param>
    /// <param name="user"><see cref="ClaimsPrincipal"/>. Logged in user.</param>
    /// <param name="mediator"><see cref="ISender"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="IResult"/>.</returns>
    public async static Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        ClaimsPrincipal user,
        [FromServices] ISender mediator,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) && userId != Guid.Empty)
        {
            return Results.Unauthorized();
        }
        
        if (id == Guid.Empty)
        {
            var error = Error.Validation(code: "DeleteProfile.IdRequired", description: "Id is required.");
            return new List<Error> { error }.ToProblemDetailsResult();
        }

        var command = new DeleteProfileCommand(id, userId);
        var result = await mediator.Send(command, cancellationToken);
        
        return result.Match(
            _ => Results.NoContent(),
            errors => errors.ToProblemDetailsResult());
    }
}