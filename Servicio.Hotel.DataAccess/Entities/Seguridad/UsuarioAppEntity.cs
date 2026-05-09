using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Seguridad
{
    public class UsuarioAppEntity
    {
        public int IdUsuario { get; set; }
        public Guid UsuarioGuid { get; set; }
        public int? IdCliente { get; set; }
        public string Username { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string EstadoUsuario { get; set; }
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string? CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}
