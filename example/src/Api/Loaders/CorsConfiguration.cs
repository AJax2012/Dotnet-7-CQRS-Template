using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleProject.Api.Loaders;

internal static class CorsConfiguration
{
    private const string AllowClientOrigin = "ExampleProjectOrigin";
    private const string AllowSwaggerOrigin = "SwaggerOrigin";

    internal static void AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        var clientRoute = configuration.GetSection("Auth:ClientRoute").Value;

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: AllowClientOrigin,
                policy =>
                {
                    policy.WithOrigins(clientRoute!);
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowCredentials();
                });
            options.AddPolicy(AllowSwaggerOrigin, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
}