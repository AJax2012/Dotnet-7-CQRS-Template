using System.Reflection;
using Ardalis.GuardClauses;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using SourceName.Api.Filters;
using SourceName.Api.Loaders;
using SourceName.Application.Common.Behaviors;
using SourceName.Application.Loaders;
using SourceName.Infrastructure.Data;
using SourceName.Infrastructure.Loaders;
using SourceName.Infrastructure.Loaders.Models;

using ILogger = Serilog.ILogger;

const string allowClientOrigin = "SourceNameOrigin";
const string allowSwaggerOrigin = "SwaggerOrigin";

var builder = WebApplication.CreateBuilder(args);

// Add logger
builder.Logging.ClearProviders();

#if EnableFileLogger
var loggingFilePath = builder.Configuration.GetSection("LoggingFilePath").Value;
Guard.Against.NullOrWhiteSpace(loggingFilePath, "LoggingFilePath",
    "When using file logger for serilog, LoggingFilePath must be set in appsettings or environment configuration");
#endif

ILogger logger = new LoggerConfiguration()
    .Enrich.WithExceptionDetails()
    .Enrich.WithDemystifiedStackTraces()
    .WriteTo.Console()
#if EnableFileLogger
    .WriteTo.File(new JsonFormatter(renderMessage: true), loggingFilePath, rollingInterval: RollingInterval.Day)
#endif
    .CreateLogger();

builder.Host.UseSerilog(logger);

// check configuration
var jwtSettings = builder.Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
var clientRoute = builder.Configuration.GetSection("Auth:ClientRoute").Value;

Guard.Against.Null(jwtSettings, "Auth:JwtBearerTokenSettings", "Jwt Settings must be set in appsettings or environment configuration");
Guard.Against.NullOrWhiteSpace(clientRoute, "Auth:ClientRoute", "Client Route must be set in appsettings or environment configuration");

var isDevelopment = builder.Environment.IsDevelopment();

// Add Business Validators
builder.Services.AddValidators();

// Add services to the container.
builder.Services.AddSingleton(logger);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SourceName", Version = "v1" });
    options.EnableAnnotations();
    options.CustomSchemaIds(type => type.ToString());
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            new string[] { }
        },
    });
});

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: allowClientOrigin,
        builder =>
        {
            builder.WithOrigins(clientRoute);
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
            builder.AllowCredentials();
        });
    options.AddPolicy(allowSwaggerOrigin, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers(opts => opts.Filters.Add(new ApiExceptionFilter(logger)));
builder.Services.AddDbContext<ExampleContext>(builder => builder.UseInMemoryDatabase("example"));
builder.Services.AddMvc().AddFluentValidation();
builder.Services.AddHttpContextAccessor();

// throttling
#if EnableRateLimiting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddRedisRateLimiting();
#endif

builder.Services.RegisterDependencies();
builder.Services.ConfigureIdentity(jwtSettings, isDevelopment, logger);
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

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(isDevelopment ? allowSwaggerOrigin : allowClientOrigin);
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();