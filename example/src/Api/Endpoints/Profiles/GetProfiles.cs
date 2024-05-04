using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using ExampleProject.Api.Endpoints.Common.Contracts;
using ExampleProject.Api.Endpoints.Common.Extensions;
using ExampleProject.Application.Profiles.Queries.GetPaginated;
using ExampleProject.Contracts.Profiles.Requests;
using ExampleProject.Contracts.Profiles.Responses;

namespace ExampleProject.Api.Endpoints.Profiles;

/// <summary>
/// Endpoint handler for getting a paginated list of Profile.
/// </summary>
public class GetProfiles : IEndpoint
{
    /// <inheritdoc />
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app) => app
        .MapGet("/", HandleAsync)
        .Accepts<GetProfilesRequest>("application/json")
        .Produces<GetProfilesResponse>(StatusCodes.Status200OK, "application/json")
        .ProducesProblem(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetProfiles");

    /// <summary>
    /// API request for getting a paginated list of Profile.
    /// </summary>
    /// <param name="limit">Number of results to retrieve.</param>
    /// <param name="orderBy">List of properties to order results by.</param>
    /// <param name="isDescending">Sets orderBy to descending or ascending (descending by default).</param>    
    /// <param name="nextPageToken">Tokenized string to get the next page.</param>
    /// <param name="user"><see cref="ClaimsPrincipal"/>. Logged in user.</param>
    /// <param name="mediator"><see cref="ISender"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns><see cref="IResult"/>.</returns>
    public async static Task<IResult> HandleAsync(
        [AsParameters] GetProfilesRequest request,
        ClaimsPrincipal user,
        [FromServices] ISender mediator,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) && userId != Guid.Empty)
        {
            return Results.Unauthorized();
        }

        var query = new GetProfilesPaginatedQuery(
            userId,
            request.Limit ?? 10,
            request.OrderBy?.ToList() ?? [],
            request.IsDescending ?? true,
            request.NextPageToken);
        
        var result = await mediator.Send(query, cancellationToken);
        
        return result.Match(
            profile => Results.Ok(profile.ToResponse()),
            errors => errors.ToProblemDetailsResult());
    }
}