using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using ExampleProject.Api.Endpoints.Common.Contracts;
using ExampleProject.Api.Endpoints.Common.Extensions;
using ExampleProject.Application.Profiles.Commands.Update;
using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Api.Endpoints.Profiles;

/// <summary>
/// Endpoint handler for updating a Profile.
/// </summary>
public class UpdateProfile : IEndpoint
{
    /// <inheritdoc />
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app) => app
        .MapPut("{id:guid}", HandleAsync)
        .WithRequestValidation<UpdateProfileRequest>()
        .Accepts<UpdateProfileRequest>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/json")
        .ProducesProblem(StatusCodes.Status404NotFound, "application/json")
        .WithName("UpdateProfile");

    /// <summary>
    /// API request for updating a Profile.
    /// </summary>
    /// <param name="id">Unique id for Profile.</param>
    /// <param name="request"><see cref="UpdateProfileRequest"/>.</param>
    /// <param name="user"><see cref="ClaimsPrincipal"/>. Logged in user.</param>
    /// <param name="mediator"><see cref="ISender"/>.</param>
    /// <param name="validator">Validator for <see cref="UpdateProfileRequest"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="IResult"/>.</returns>
    public async static Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProfileRequest request,
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
            var error = Error.Validation(code: "UpdateProfile.IdRequired", description: "Id is required.");
            return new List<Error> { error }.ToProblemDetailsResult();
        }

        var command = new UpdateProfileCommand(id, userId);

        var result = await mediator.Send(command, cancellationToken);
        
        return result.Match(
            _ => Results.NoContent(),
            errors => errors.ToProblemDetailsResult());
    }
}