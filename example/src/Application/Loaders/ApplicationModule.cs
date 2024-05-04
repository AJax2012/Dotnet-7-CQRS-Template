using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleProject.Application.Loaders;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.RegisterDependencies();
        
        return services;
    }

    private static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        return services;
    }
}