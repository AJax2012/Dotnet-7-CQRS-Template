using Microsoft.Extensions.DependencyInjection;
using SourceName.Application.Contracts;
using SourceName.Infrastructure.Data;

namespace SourceName.Infrastructure.Loaders;

internal static class DependencyInjection
{
    internal static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddTransient<IRepository, ExampleRepository>();
    }
}