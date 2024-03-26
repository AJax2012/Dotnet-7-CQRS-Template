using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SourceName.Api.Endpoints.Common;

internal static class EndpointExtensions
{
    internal static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        var endpointTypes = GetEndpointTypesFromContainingAssembly<TMarker>();

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoint.DefineEndpoints))!
                .Invoke(null, new object[] { app });
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromContainingAssembly<TMarker>()
    {
        return typeof(TMarker).Assembly.DefinedTypes
            .Where(x => !x.IsAbstract &&
                        !x.IsInterface &&
                        typeof(IEndpoint).IsAssignableFrom(x));
    }
}