using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SourceName.Infrastructure.Loaders.Models;
using ILogger = Serilog.ILogger;

namespace SourceName.Api.Loaders;

internal static class IdentityConfiguration
{
    public static void ConfigureIdentity(
        this IServiceCollection services,
        JwtBearerTokenSettings jwtSettings,
        bool isDevelopment,
        ILogger logger)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !isDevelopment;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(2),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        else
                        {
                            logger.Error("JWT Token Errror: {Message}", context.Exception.Message);
                            context.Response.Headers.Append("Token-Error", "invalid token");
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = _ => Task.CompletedTask,
                };
            });
    }
}