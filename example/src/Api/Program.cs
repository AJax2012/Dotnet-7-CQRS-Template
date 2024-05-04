using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ExampleProject.Api;
using ExampleProject.Api.Endpoints.Common.Extensions;
using ExampleProject.Api.Loaders;
using ExampleProject.Application.Loaders;
using ExampleProject.Infrastructure.Loaders;

const string allowClientOrigin = "ExampleProjectOrigin";
const string allowSwaggerOrigin = "SwaggerOrigin";

var builder = WebApplication.CreateBuilder(args);
{
    var logger = builder.AddLogging();

    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);

    // Add services to the container.
    builder.Services.AddSingleton(logger);
    
    // Add health checks
    builder.Services.AddHealthChecks();

    // Add Endpoint Registration
    builder.Services.AddEndpointsApiExplorer();

    // Add cors
    builder.Services.AddCors(builder.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddSwagger();

    builder.Services.AddHttpContextAccessor();


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