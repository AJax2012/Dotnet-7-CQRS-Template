using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SourceName.Api;
using SourceName.Api.Endpoints.Common.Extensions;
using SourceName.Api.Loaders;
using SourceName.Application.Loaders;
using SourceName.Infrastructure.Loaders;

const string allowClientOrigin = "SourceNameOrigin";
const string allowSwaggerOrigin = "SwaggerOrigin";

var builder = WebApplication.CreateBuilder(args);
{
    var logger = builder.AddLogging();

    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);

    // Add services to the container.
    builder.Services.AddSingleton(logger);
    
    // Add health checks
#if EnableRateLimiting
    builder.Services.AddHealthChecks()
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
#else
    builder.Services.AddHealthChecks();
#endif

    // Add Endpoint Registration
    builder.Services.AddEndpointsApiExplorer();

    // Add cors
    builder.Services.AddCors(builder.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddSwagger();

    builder.Services.AddHttpContextAccessor();

#if EnableRateLimiting
    // throttling
    builder.Services.AddRateLimiting(builder.Configuration);
#endif

    builder.Services.RegisterDependencies()
        .AddInfrastructureModule()
        .AddApplicationModule();
}

var app = builder.Build();
{
    var isDevelopment = builder.Environment.IsDevelopment();
    
    // Configure the HTTP request pipeline.
    if (isDevelopment)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    // Add health checks
    // TODO: add health checks for data providers.
    app.MapHealthChecks("/_health");
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors(isDevelopment ? allowSwaggerOrigin : allowClientOrigin);

    app.UseEndpoints<IApiMarker>();

    app.Run();
}