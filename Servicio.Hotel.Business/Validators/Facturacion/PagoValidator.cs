using System.Collections.Generic;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Facturacion
{
    public static class PagoValidator
    {
        public static void Validate(PagoDTO pago)
        {
            if (pago == null)
                throw new ValidationException("PAG-001", "El pago no puede ser nulo.");

            var errors = new Dictionary<string, string[]>();

            if (pago.IdFactura <= 0)
                errors["IdFactura"] = new[] { "El id de factura es obligatorio." };

            if (pago.IdReserva <= 0)
                errors["IdReserva"] = new[] { "El id de reserva es obligatorio." };

            if (pago.Monto <= 0)
                errors["Monto"] = new[] { "El monto del pago debe ser mayor a cero." };

            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
                errors["MetodoPago"] = new[] { "El método de pago es obligatorio." };
            else if (pago.MetodoPago.Length > 40)
                errors["MetodoPago"] = new[] { "El método de pago no puede exceder 40 caracteres." };

            var estadosValidos = new[] { "PEN", "PRO", "APR", "REC", "CAN" };
            if (!estadosValidos.Contains(pago.EstadoPago))
                errors["EstadoPago"] = new[] { $"Estado de pago inválido. Valores permitidos: {string.Join(", ", estadosValidos)}." };

            if (pago.EsPagoElectronico)
            {
                if (string.IsNullOrWhiteSpace(pago.ProveedorPasarela))
                    errors["ProveedorPasarela"] = new[] { "El proveedor de pasarela es obligatorio para pagos electrónicos." };
                else if (pago.ProveedorPasarela.Length > 50)
                    errors["ProveedorPasarela"] = new[] { "El proveedor no puede exceder 50 caracteres." };

                if (string.IsNullOrWhiteSpace(pago.TransaccionExterna))
                    errors["TransaccionExterna"] = new[] { "La transacción externa es obligatoria para pagos electrónicos." };
                else if (pago.TransaccionExterna.Length > 150)
                    errors["TransaccionExterna"] = new[] { "La transacción externa no puede exceder 150 caracteres." };
            }

            if (errors.Count > 0)
                throw new ValidationException("PAG-002", errors);
        }
    }
}