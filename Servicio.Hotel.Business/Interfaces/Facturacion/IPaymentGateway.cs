using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.Business.Interfaces.Facturacion
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResult> ProcesarPagoAsync(PaymentGatewayRequest request, CancellationToken ct = default);
    }

    public sealed class PaymentGatewayRequest
    {
        public int IdReserva { get; set; }
        public int IdFactura { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Moneda { get; set; } = "USD";
        public string Usuario { get; set; } = "Sistema";
        public string Referencia { get; set; } = string.Empty;
        public string? TokenPago { get; set; }
    }

    public sealed class PaymentGatewayResult
    {
        public bool Aprobado { get; set; }
        public string Estado { get; set; } = "REC";
        public string TransaccionExterna { get; set; } = string.Empty;
        public string CodigoAutorizacion { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string RespuestaRaw { get; set; } = string.Empty;
    }
}
