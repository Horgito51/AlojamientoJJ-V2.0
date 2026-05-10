using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataManagement.Alojamiento.Models
{
    public class TipoHabitacionDataModel
    {
        public int IdTipoHabitacion { get; set; }
        public Guid TipoHabitacionGuid { get; set; }
        public string CodigoTipoHabitacion { get; set; }
        public string NombreTipoHabitacion { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Descripcion { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public int CapacidadTotal { get; set; }
        public string TipoCama { get; set; }
        public decimal? AreaM2 { get; set; }
        public bool PermiteEventos { get; set; }
        public bool PermiteReservaPublica { get; set; }
        public string EstadoTipoHabitacion { get; set; }
        public bool EsEliminado { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }
        public List<ImagenDataModel> Imagenes { get; set; } = new();
    }
}
