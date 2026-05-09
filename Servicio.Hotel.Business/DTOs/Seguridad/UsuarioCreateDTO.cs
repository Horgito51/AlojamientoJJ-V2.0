using System.Collections.Generic;
using System;

namespace Servicio.Hotel.Business.DTOs.Seguridad
{
    public class UsuarioCreateDTO
    {
        public int? IdCliente { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string EstadoUsuario { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
        public int? IdRol { get; set; }
        public Guid? RolGuid { get; set; }
        public List<RolDTO> Roles { get; set; } = new();
    }
}
