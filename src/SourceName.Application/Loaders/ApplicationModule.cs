using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SourceName.Application.Common.Mapping;

namespace SourceName.Application.Loaders;

public static class ApplicationModule
{
    public static void AddApplicationModule(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(executingAssembly);
        services.AddAutoMapper(typeof(MappingProfile));
    }
}