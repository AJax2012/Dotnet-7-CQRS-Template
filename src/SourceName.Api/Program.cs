using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SourceName.Api.Endpoints.Internal;
using SourceName.Api.Filters;
using SourceName.Api.Loaders;
using SourceName.Application.Common.Behaviors;
using SourceName.Application.Loaders;
using SourceName.Infrastructure.Data;
using SourceName.Infrastructure.Loaders;
using SourceName.Infrastructure.Loaders.Models;

const string allowClientOrigin = "SourceNameOrigin";
const string allowSwaggerOrigin = "SwaggerOrigin";

var builder = WebApplication.CreateBuilder(args);
var logger = builder.AddLogging();

// check configuration
var jwtSettings = builder.Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
var isDevelopment = builder.Environment.IsDevelopment();

// Add Business Validators
builder.Services.AddValidators();

// Add services to the container.
builder.Services.AddSingleton(logger);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Add Endpoint Registration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpoints<Program>(builder.Configuration);

// Add cors
builder.Services.AddCors(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

builder.Services.AddDbContext<ExampleContext>(opts => opts.UseInMemoryDatabase("example"));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();

// throttling
#if EnableRateLimiting
builder.Services.AddRateLimiting(builder.Configuration);
#endif

builder.Services.RegisterDependencies();
builder.Services.ConfigureIdentity(jwtSettings!, isDevelopment, logger);
builder.Services.AddInfrastructureModule();
builder.Services.AddApplicationModule();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(isDevelopment ? allowSwaggerOrigin : allowClientOrigin);

// app.UseAuthorization();
app.UseEndpoints<Program>();

app.Run();