namespace Servicio.Hotel.API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, string[] allowedOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    if (allowedOrigins.Length == 1 && allowedOrigins[0] == "*")
                    {
                        policy.AllowAnyOrigin();
                    }
                    else
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowCredentials();
                    }

                    policy.AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}