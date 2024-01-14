using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SourceName.Api;
using SourceName.Api.Endpoints.Common;
using SourceName.Api.Loaders;
using SourceName.Application.Loaders;
using SourceName.Infrastructure.Loaders;
using SourceName.Infrastructure.Loaders.Models;

const string allowClientOrigin = "SourceNameOrigin";
const string allowSwaggerOrigin = "SwaggerOrigin";

var builder = WebApplication.CreateBuilder(args);
{
    var logger = builder.AddLogging();

    // check configuration
    var jwtSettings = builder.Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
    var isDevelopment = builder.Environment.IsDevelopment();

#if EnableAuthenticationEndpoints
    // add identity
    // TODO: add sql provider
    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);

    builder.Services.AddAuthorizationBuilder();

    builder.Services.AddIdentityCore<AppUser>()
        .AddEntityFrameworkStores<SourceNameDbContext>()
        .AddApiEndpoints();
#endif

    // Add services to the container.
    builder.Services.AddSingleton(logger);
    
    // Add health checks
    builder.Services.AddHealthChecks();
#if EnableRateLimiting
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
#endif

    // Add Endpoint Registration
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddEndpoints<Program>(builder.Configuration);

    // Add cors
    builder.Services.AddCors(builder.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddSwagger();

    builder.Services.AddHttpContextAccessor();

// throttling
#if !EnableRateLimiting
    builder.Services.AddRateLimiting(builder.Configuration);
#endif

    builder.Services.RegisterDependencies()
        .AddInfrastructureModule()
        .AddApplicationModule();
    
#if EnableAuthenticationEndpoints
    builder.Services.ConfigureIdentity(jwtSettings!, isDevelopment, logger);
#endif
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

#if EnableAuthenticationEndpoints
    app.MapIdentityApi<AppUser>();
#endif

    app.MapHealthChecks("/_health");
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors(isDevelopment ? allowSwaggerOrigin : allowClientOrigin);

    app.UseEndpoints<IApiMarker>();

    app.Run();
}