using System;

namespace Servicio.Hotel.DataAccess.Entities.Seguridad
{
    public class UsuarioRolEntity
    {
        public int IdUsuarioRol { get; set; }
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string EstadoUsuarioRol { get; set; }
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public byte[] RowVersion { get; set; }

        public UsuarioAppEntity UsuarioApp { get; set; }
        public RolEntity Rol { get; set; }
    }
}
