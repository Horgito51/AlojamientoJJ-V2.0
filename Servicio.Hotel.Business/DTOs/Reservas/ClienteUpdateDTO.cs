namespace Servicio.Hotel.Business.DTOs.Reservas
{
    public class ClienteUpdateDTO
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = "ACT";
    }
}
