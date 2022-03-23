using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SourceName.Infrastructure.Data;

namespace SourceName.Infrastructure.Loaders;

public static class InfrastructureModule
{
    public static void AddInfrastructureModule(this IServiceCollection services)
    {
        services.RegisterDependencies();
        // services.AddDbContext<ExampleContext>(options => options.UseInMemoryDatabase("examples"));
    }
}