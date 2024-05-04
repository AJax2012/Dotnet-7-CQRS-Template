using Microsoft.Extensions.DependencyInjection;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Infrastructure.Persistence.Profiles;

namespace ExampleProject.Infrastructure.Loaders;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
    {
        services.RegisterDependencies();
        
        return services;
    }

    private static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProfilesRepository, ProfilesRepository>();

        return services;
    }
}
