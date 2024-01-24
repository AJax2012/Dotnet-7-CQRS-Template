using Microsoft.Extensions.DependencyInjection;

namespace SourceName.Infrastructure.Loaders;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
    {
        services.RegisterDependencies();
        
        return services;
    }

    private static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        return services;
    }
}