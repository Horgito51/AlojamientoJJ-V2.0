using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Extensions;
using Servicio.Hotel.API.Filters;
using Servicio.Hotel.API.Middleware;
using Servicio.Hotel.API.Models.Settings;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar configuraciones
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🔥 VALIDACIONES SEGURAS (no rompen producción)

if (jwtSettings is null)
{
    throw new InvalidOperationException("La configuración 'Jwt' es obligatoria.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Secret) || jwtSettings.Secret.Length < 32)
{
    throw new InvalidOperationException("La configuración 'Jwt:Secret' debe tener al menos 32 caracteres.");
}

// 👉 fallback si no hay CORS (evita crash en Azure)
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    allowedOrigins = new[] { "*" };
}

// 👉 fallback si no hay conexión (mejor error claro)
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' es obligatoria.");
}

// Registrar JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// 2. Servicios
builder.Services.AddDataAccessServices(connectionString);
builder.Services.AddJwtAuthentication(jwtSettings, builder.Environment);
builder.Services.AddCustomAuthorization();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCustomCors(allowedOrigins);
builder.Services.AddApiVersioningConfiguration();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

var app = builder.Build();

// 3. Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 🔥 Swagger SIEMPRE activo (para pruebas en Azure)
app.UseSwagger();
app.UseSwaggerUI();

// 👉 HTTPS en producción
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseMiddleware<AdminProfileAccessMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();