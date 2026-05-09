using System;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class HabitacionEntity
    {
        public int IdHabitacion { get; set; }
        public Guid HabitacionGuid { get; set; }
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; }
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? Url { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; }
        public bool EsEliminado { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }

        public SucursalEntity Sucursal { get; set; }
        public TipoHabitacionEntity TipoHabitacion { get; set; }
    }
}
