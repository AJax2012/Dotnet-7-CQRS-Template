using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using SourceName.Api.Endpoints.Common.Contracts;
using SourceName.Api.Endpoints.Common.Filters;

namespace SourceName.Api.Endpoints.Common.Extensions;

internal static class EndpointExtensions
{
    /// <summary>
    /// Maps endpoints from the assembly containing <typeparamref name="TMarker"/>
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    /// <typeparam name="TMarker"></typeparam>
    /// <returns></returns>
    internal static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        var endpointTypes = GetEndpointTypesFromContainingAssembly<TMarker>();

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IFeatureEndpoints.MapEndpoints))!
                .Invoke(null, [app]);
        }
    }

    /// <summary>
    /// Used on an endpoint to add a request validation filter to the route handler.
    /// </summary>
    /// <example>
    /// <code>
    /// app.MapGet("{id}", Handle)
    ///    .WithRequestValidation&lt;SomeRequest&gt;();
    /// </code>
    /// </example>
    /// <param name="builder"><see cref="RouteHandlerBuilder"/>.</param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
    internal static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
        where TRequest : IRequest =>
            builder
                .AddEndpointFilter<RequestValidationFilter<TRequest>>()
                .ProducesValidationProblem();

    /// <summary>
    /// Maps an endpoint to a <see cref="RouteGroupBuilder"/> or an <see cref="IFeatureEndpoints"/> class.
    /// </summary>
    /// <example>
    /// <code>
    /// app.MapGroup("/api")
    ///    .MapEndpoint&lt;SomeEndpoint&gt;();
    /// </code>
    /// </example>
    /// <param name="app"><see cref="IEndpointRouteBuilder"/>.</param>
    /// <typeparam name="TEndpoint">Endpoint class that implements <see cref="IEndpoint"/>.</typeparam>
    /// <returns></returns>
    internal static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) 
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromContainingAssembly<TMarker>() =>
        typeof(TMarker).Assembly.DefinedTypes
            .Where(x => x is { IsAbstract: false, IsInterface: false } &&
                typeof(IFeatureEndpoints).IsAssignableFrom(x));
}