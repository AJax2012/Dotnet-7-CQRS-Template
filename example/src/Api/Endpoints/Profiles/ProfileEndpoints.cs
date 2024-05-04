using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using ExampleProject.Api.Endpoints.Common.Contracts;
using ExampleProject.Api.Endpoints.Common.Extensions;

namespace ExampleProject.Api.Endpoints.Profiles;

/// <summary>
/// Defines Profile Feature endpoints.
/// </summary>
public class ProfileEndpoints : IFeatureEndpoints
{
    /// <inheritdoc />
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGroup("api/Profiles")
            .WithTags(["Profile"])
            .RequireAuthorization()
            .MapEndpoint<CreateProfile>()
            .MapEndpoint<GetProfileById>()
            .MapEndpoint<GetProfiles>()
            .MapEndpoint<UpdateProfile>()
            .MapEndpoint<DeleteProfile>();            
    }
}