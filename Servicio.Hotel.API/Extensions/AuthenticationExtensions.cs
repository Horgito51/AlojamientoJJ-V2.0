using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.API.Models.Settings;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings, IWebHostEnvironment environment)
        {
            if (string.IsNullOrWhiteSpace(jwtSettings.Secret) || jwtSettings.Secret.Length < 32)
                throw new InvalidOperationException("La configuracion 'Jwt:Secret' debe tener al menos 32 caracteres.");

            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !environment.IsDevelopment();
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Log detallado del error de autenticación en desarrollo
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var loggerFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("JwtBearer");
                        logger.LogWarning("JWT auth failed: {Error}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // Evita la respuesta por defecto sin body
                        context.HandleResponse();

                        var loggerFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger("JwtBearer");
                        logger.LogWarning("JWT challenge: {Error} | {ErrorDescription}",
                            context.Error, context.ErrorDescription);

                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var errorResponse = new ApiErrorResponse(
                                message: "No autorizado. Se requiere token de autenticación válido.",
                                statusCode: 401,
                                errors: null,
                                traceId: context.HttpContext.TraceIdentifier
                            );

                            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });

                            return context.Response.WriteAsync(json);
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
