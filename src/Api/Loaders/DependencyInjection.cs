using Microsoft.Extensions.DependencyInjection;

namespace SourceName.Api.Loaders;

internal static class DependencyInjection
{
    internal static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<IApiMarker>();
        });
        
        return services;
    }
}