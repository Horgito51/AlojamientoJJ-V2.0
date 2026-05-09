namespace Servicio.Hotel.Business.DTOs.Seguridad
{
    public class RolUpdateDTO
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string DescripcionRol { get; set; } = string.Empty;
        public string EstadoRol { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
    }
}
