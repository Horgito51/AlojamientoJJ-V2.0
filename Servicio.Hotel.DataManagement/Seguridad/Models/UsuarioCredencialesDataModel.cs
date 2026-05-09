using System;

namespace Servicio.Hotel.DataManagement.Seguridad.Models
{
    public class UsuarioCredencialesDataModel
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string EstadoUsuario { get; set; }
        public bool Activo { get; set; }
    }
}
