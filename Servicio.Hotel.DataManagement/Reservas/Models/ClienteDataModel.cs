using System;

namespace Servicio.Hotel.DataManagement.Reservas.Models
{
    public class ClienteDataModel
    {
        public int IdCliente { get; set; }
        public Guid ClienteGuid { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RazonSocial { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public bool EsEliminado { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }
        public byte[] RowVersion { get; set; }
    }
}