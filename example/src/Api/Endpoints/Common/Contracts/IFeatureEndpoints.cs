using Microsoft.AspNetCore.Routing;

namespace ExampleProject.Api.Endpoints.Common.Contracts;

/// <summary>
/// Interface for a collection of endpoints for a feature.
/// </summary>
public interface IFeatureEndpoints
{
    /// <summary>
    /// Defines the endpoints for a feature of the application.
    /// </summary>
    /// <param name="app"></param>
    public static abstract void MapEndpoints(IEndpointRouteBuilder app);
}