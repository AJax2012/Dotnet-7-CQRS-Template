using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SourceName.Api.Endpoints.Common.Contracts;

/// <summary>
/// Marker interface for an Endpoint.
/// </summary>
public interface IEndpoint
{
	/// <summary>
	/// Defines an endpoint.
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static abstract RouteHandlerBuilder Map(IEndpointRouteBuilder app);
}