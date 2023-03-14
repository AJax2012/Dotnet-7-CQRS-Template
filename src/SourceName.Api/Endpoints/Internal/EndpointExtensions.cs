using System.Reflection;

namespace SourceName.Api.Endpoints.Internal;

internal static class EndpointExtensions
{
    internal static void AddEndpoints<TMarker>(this IServiceCollection services, IConfiguration configuration)
    {
        var endpointTypes = GetEndpointTypesFromContainingAssembly<TMarker>();

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoint.AddServices))!
                .Invoke(null, new object[] { services, configuration });
        }
    }

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