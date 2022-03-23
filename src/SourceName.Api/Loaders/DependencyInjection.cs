using SourceName.Api.Services;
using SourceName.Application.Contracts;

namespace SourceName.Api.Loaders;

internal static class DependencyInjection
{
    internal static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
    }
}