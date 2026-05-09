using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Seguridad
{
    public class UsuarioUpdateDTO
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string EstadoUsuario { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
        public List<RolDTO> Roles { get; set; } = new();
        public byte[] RowVersion { get; set; }
    }
}
