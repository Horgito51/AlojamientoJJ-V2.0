using System;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class TarifaEntity
    {
        public int IdTarifa { get; set; }
        public Guid TarifaGuid { get; set; }
        public string CodigoTarifa { get; set; }
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NombreTarifa { get; set; }
        public string CanalTarifa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public decimal PorcentajeIva { get; set; }
        public int MinNoches { get; set; }
        public int? MaxNoches { get; set; }
        public bool PermitePortalPublico { get; set; }
        public int Prioridad { get; set; }
        public string EstadoTarifa { get; set; }
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
