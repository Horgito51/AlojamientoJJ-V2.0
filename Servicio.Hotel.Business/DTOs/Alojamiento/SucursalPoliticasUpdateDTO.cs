namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class SucursalPoliticasUpdateDTO
    {
        public string? HoraCheckin { get; set; }
        public string? HoraCheckout { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
        public bool AceptaNinos { get; set; }
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
    }
}

