using System.Reflection;
using FluentValidation;
using SourceName.Application.Contracts;

namespace SourceName.Api.Loaders;

public static class ValidationConfiguration
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.Scan(scan =>
            scan.FromAssemblyOf<IValidationHandler>()
                .AddClasses(classes => classes.AssignableTo<IValidationHandler>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
    }
}