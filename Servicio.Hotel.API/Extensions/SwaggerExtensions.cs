using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Servicio.Hotel.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Servicio.Hotel.API",
                    Version = "v1",
                    Description = "API para gestión de hoteles, reservas y facturación"
                });
                c.SchemaFilter<AccommodationDetailSchemaFilter>();

                // 🔐 JWT en Swagger - esquema Http estándar
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese solo el token JWT (sin 'Bearer ').\nSwagger agrega el prefijo automáticamente."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }

    public sealed class AccommodationDetailSchemaFilter : Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext context)
        {
            if (context.Type.FullName != "Servicio.Hotel.Business.DTOs.Booking.AccommodationDetailResponseDTO")
                return;

            schema.Properties.Remove("disponibilidad");
        }
    }
}
