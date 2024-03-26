using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SourceName.Api.Endpoints.Common;

/// <summary>
/// Marker interface for Endpoints.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Defines the endpoints for the application.
    /// </summary>
    /// <param name="app"></param>
    public static abstract void DefineEndpoints(IEndpointRouteBuilder app);
}