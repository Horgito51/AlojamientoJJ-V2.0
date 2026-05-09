using System;

namespace Servicio.Hotel.DataAccess.Entities.Valoraciones
{
    public class ValoracionEntity
    {
        public int IdValoracion { get; set; }
        public Guid ValoracionGuid { get; set; }
        public int IdEstadia { get; set; }
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public int? IdHabitacion { get; set; }
        public decimal PuntuacionGeneral { get; set; }
        public decimal? PuntuacionLimpieza { get; set; }
        public decimal? PuntuacionConfort { get; set; }
        public decimal? PuntuacionUbicacion { get; set; }
        public decimal? PuntuacionInstalaciones { get; set; }
        public decimal? PuntuacionPersonal { get; set; }
        public decimal? PuntuacionCalidadPrecio { get; set; }
        public string? ComentarioPositivo { get; set; }
        public string? ComentarioNegativo { get; set; }
        public string? TipoViaje { get; set; }
        public string EstadoValoracion { get; set; }
        public bool PublicadaEnPortal { get; set; }
        public string? RespuestaHotel { get; set; }
        public DateTime? FechaRespuestaUtc { get; set; }
        public string? ModeradaPorUsuario { get; set; }
        public string? MotivoModeracion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
