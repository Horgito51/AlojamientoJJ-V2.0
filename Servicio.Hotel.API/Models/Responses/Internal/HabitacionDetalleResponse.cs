using System;
using System.Collections.Generic;
using Servicio.Hotel.API.Models.Responses.Public;

namespace Servicio.Hotel.API.Models.Responses.Internal
{
    public sealed class HabitacionDetalleResponse
    {
        public Guid HabitacionGuid { get; set; }
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? ImagenUrl { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; } = string.Empty;
        public Guid SucursalGuid { get; set; }
        public TipoHabitacionRef TipoHabitacion { get; set; } = new();
        public List<ImagenPublicDto> Imagenes { get; set; } = new();
    }

    public sealed class TipoHabitacionRef
    {
        public Guid TipoHabitacionGuid { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string NombreTipoHabitacion { get; set; } = string.Empty;
    }
}
