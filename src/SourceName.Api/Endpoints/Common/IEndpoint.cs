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
    
    /// <summary>
    /// Adds services to the service collection for dependency injection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static abstract void AddServices(IServiceCollection services, IConfiguration configuration);
}