using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using ExampleProject.Api.Endpoints.Common.Contracts;
using ExampleProject.Api.Endpoints.Common.Extensions;
using ExampleProject.Application.Profiles.Commands.Create;
using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Api.Endpoints.Profiles;

/// <summary>
/// Endpoint handler for creating a Profile.
/// </summary>
public class CreateProfile : IEndpoint
{
    /// <inheritdoc />
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app) => app
        .MapPost("/", HandleAsync)
        .WithRequestValidation<CreateProfileRequest>()
        .Accepts<CreateProfileRequest>("application/json")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict, "application/json")
        .WithName("CreateProfile");

    /// <summary>
    /// API request for creating a Profile.
    /// </summary>
    /// <param name="request"><see cref="CreateProfileRequest"/>.</param>
    /// <param name="user"><see cref="ClaimsPrincipal"/>. Logged in user.</param>
    /// <param name="mediator"><see cref="ISender"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="IResult"/>.</returns>
    public async static Task<IResult> HandleAsync(
        [FromBody] CreateProfileRequest request,
        ClaimsPrincipal user,
        [FromServices] ISender mediator,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) && userId != Guid.Empty)
        {
            return Results.Unauthorized();
        }
    
        var command = new CreateProfileCommand();

        var result = await mediator.Send(command, cancellationToken);
        
        return result.Match(
            id => Results.CreatedAtRoute("GetProfileById", new { id = id }),
            errors => errors.ToProblemDetailsResult());
    }
}