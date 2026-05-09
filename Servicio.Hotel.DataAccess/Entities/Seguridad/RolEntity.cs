using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Seguridad
{
    public class RolEntity
    {
        public int IdRol { get; set; }
        public Guid RolGuid { get; set; }
        public string NombreRol { get; set; }
        public string? DescripcionRol { get; set; }
        public string EstadoRol { get; set; }
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}
