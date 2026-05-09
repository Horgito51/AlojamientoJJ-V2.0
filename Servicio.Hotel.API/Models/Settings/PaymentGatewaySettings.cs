namespace Servicio.Hotel.API.Models.Settings
{
    public class PaymentGatewaySettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ChargePath { get; set; } = "/payments/charge";
        public string ApiKey { get; set; } = string.Empty;
        public string ProviderName { get; set; } = "EXTERNAL";
    }
}
