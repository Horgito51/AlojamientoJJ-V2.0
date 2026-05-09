using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Seguridad
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public int? IdCliente { get; set; }
        public Guid? ClienteGuid { get; set; }
        public Guid UsuarioGuid { get; set; }
        public string Username { get; set; }
        public string Correo { get; set; }
        public string NombreCompleto { get; set; }
        public List<string> Roles { get; set; }
    }
}
