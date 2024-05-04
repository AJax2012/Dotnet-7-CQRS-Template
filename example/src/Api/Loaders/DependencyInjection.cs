using Microsoft.Extensions.DependencyInjection;

namespace ExampleProject.Api.Loaders;

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