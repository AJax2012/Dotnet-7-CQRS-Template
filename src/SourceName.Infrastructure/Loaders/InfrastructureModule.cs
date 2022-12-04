using Microsoft.Extensions.DependencyInjection;

namespace SourceName.Infrastructure.Loaders;

public static class InfrastructureModule
{
    public static void AddInfrastructureModule(this IServiceCollection services)
    {
        services.RegisterDependencies();

        // services.AddDbContext<ExampleContext>(options => options.UseInMemoryDatabase("examples"));
    }
}