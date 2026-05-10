using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.API.Models.Requests.Public
{
    public sealed class PublicReservaCreateRequest
    {
        public Guid ClienteGuid { get; set; }
        public Guid SucursalGuid { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public string? Observaciones { get; set; }
        public bool EsWalkin { get; set; }
        public string? OrigenCanalReserva { get; set; }
        public PublicClienteCreateRequest? Cliente { get; set; }
        public List<PublicReservaHabitacionCreateRequest> Habitaciones { get; set; } = new();

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraProperties { get; set; }

        public void ValidateNoIds()
        {
            PublicRequestGuard.RejectIdProperties(ExtraProperties);
            foreach (var habitacion in Habitaciones)
            {
                habitacion.ValidateNoIds();
            }
        }
    }

    public sealed class PublicReservaHabitacionCreateRequest
    {
        public Guid HabitacionGuid { get; set; }
        public Guid TipoHabitacionGuid { get; set; }
        public int NumHabitaciones { get; set; } = 1;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int NumAdultos { get; set; } = 1;
        public int NumNinos { get; set; }
        public decimal DescuentoLinea { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraProperties { get; set; }

        public void ValidateNoIds()
            => PublicRequestGuard.RejectIdProperties(ExtraProperties);
    }

    public sealed class PublicClienteCreateRequest
    {
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }
    }

    public sealed class PublicReservaPrecioRequest
    {
        public Guid HabitacionGuid { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Canal { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraProperties { get; set; }

        public void ValidateNoIds()
            => PublicRequestGuard.RejectIdProperties(ExtraProperties);
    }

    public sealed class PublicPagoSimularRequest
    {
        public Guid ReservaGuid { get; set; }
        public decimal? Monto { get; set; }
        public string MetodoPago { get; set; } = "TARJETA";
        public bool EsPagoElectronico { get; set; } = true;
        public string? ProveedorPasarela { get; set; } = "EXTERNAL";
        public string? TransaccionExterna { get; set; }
        public string? CodigoAutorizacion { get; set; }
        public string? TokenPago { get; set; }
        public string? Referencia { get; set; }
        public string EstadoPago { get; set; } = "PEN";
        public DateTime? FechaPagoUtc { get; set; }
        public string Moneda { get; set; } = "USD";
        public decimal TipoCambio { get; set; } = 1m;
        public string? RespuestaPasarela { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraProperties { get; set; }

        public void ValidateNoIds()
            => PublicRequestGuard.RejectIdProperties(ExtraProperties);
    }

    public sealed class PublicCancelarReservaRequest
    {
        public string? Motivo { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraProperties { get; set; }

        public void ValidateNoIds()
            => PublicRequestGuard.RejectIdProperties(ExtraProperties);
    }

    internal static class PublicRequestGuard
    {
        public static void RejectIdProperties(Dictionary<string, JsonElement>? extraProperties)
        {
            if (extraProperties is null || extraProperties.Count == 0) return;

            foreach (var propertyName in extraProperties.Keys)
            {
                var normalized = propertyName.Trim();
                if (IsIdProperty(normalized))
                {
                    throw new ValidationException(
                        "PUB-GUID-001",
                        $"El contrato publico no acepta IDs numericos. Use GUIDs para el campo '{propertyName}'.");
                }
            }
        }

        public static bool IsIdProperty(string propertyName)
        {
            var normalized = propertyName.Trim();
            return normalized.Equals("id", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("id", StringComparison.OrdinalIgnoreCase) ||
                normalized.EndsWith("id", StringComparison.OrdinalIgnoreCase);
        }
    }
}
