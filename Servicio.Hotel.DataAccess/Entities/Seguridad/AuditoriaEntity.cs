using System;

namespace Servicio.Hotel.DataAccess.Entities.Seguridad
{
    public class AuditoriaEntity
    {
        public long IdAuditoria { get; set; }
        public Guid AuditoriaGuid { get; set; }
        public string TablaAfectada { get; set; }
        public string Operacion { get; set; }
        public string? IdRegistroAfectado { get; set; }
        public string? DatosAnteriores { get; set; }
        public string? DatosNuevos { get; set; }
        public string UsuarioEjecutor { get; set; }
        public string? IpOrigen { get; set; }
        public string ServicioOrigen { get; set; }
        public DateTime FechaEventoUtc { get; set; }
        public bool Activo { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
