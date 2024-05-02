using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
    /// <param name="builder"><see cref="RouteHandlerBuilder"/>.</param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
    internal static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
        where TRequest : IRequest =>
            builder
                .AddEndpointFilter<RequestValidationFilter<TRequest>>()
                .ProducesValidationProblem();

    private static IEnumerable<TypeInfo> GetEndpointTypesFromContainingAssembly<TMarker>() =>
        typeof(TMarker).Assembly.DefinedTypes
            .Where(x => x is { IsAbstract: false, IsInterface: false } &&
                typeof(IFeatureEndpoints).IsAssignableFrom(x));
}