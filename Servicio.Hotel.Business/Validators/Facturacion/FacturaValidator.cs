using System;
using System.Collections.Generic;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Facturacion
{
    public static class FacturaValidator
    {
        public static void Validate(FacturaDTO factura)
        {
            if (factura == null)
                throw new ValidationException("FAC-001", "La factura no puede ser nula.");

            var errors = new Dictionary<string, string[]>();

            if (factura.IdCliente <= 0)
                errors["IdCliente"] = new[] { "El id del cliente es obligatorio." };

            if (factura.IdReserva <= 0)
                errors["IdReserva"] = new[] { "El id de la reserva es obligatorio." };

            if (factura.IdSucursal <= 0)
                errors["IdSucursal"] = new[] { "El id de la sucursal es obligatorio." };

            if (string.IsNullOrWhiteSpace(factura.NumeroFactura))
                errors["NumeroFactura"] = new[] { "El número de factura es obligatorio." };
            else if (factura.NumeroFactura.Length > 40)
                errors["NumeroFactura"] = new[] { "El número de factura no puede exceder 40 caracteres." };

            var tiposValidos = new[] { "RESERVA", "FINAL", "AJUSTE" };
            if (!tiposValidos.Contains(factura.TipoFactura))
                errors["TipoFactura"] = new[] { $"Tipo de factura inválido. Valores permitidos: {string.Join(", ", tiposValidos)}." };

            if (factura.Subtotal < 0)
                errors["Subtotal"] = new[] { "El subtotal no puede ser negativo." };

            if (factura.ValorIva < 0)
                errors["ValorIva"] = new[] { "El IVA no puede ser negativo." };

            if (factura.Total < 0)
                errors["Total"] = new[] { "El total no puede ser negativo." };

            decimal totalCalculado = factura.Subtotal + factura.ValorIva - factura.DescuentoTotal;
            if (factura.Total != totalCalculado)
                errors["Total"] = new[] { $"El total no coincide con subtotal + IVA - descuento. Esperado: {totalCalculado}." };

            var estadosValidos = new[] { "EMI", "PAG", "ANU" };
            if (!estadosValidos.Contains(factura.Estado))
                errors["Estado"] = new[] { $"Estado de factura inválido. Valores permitidos: {string.Join(", ", estadosValidos)}." };

            if (errors.Count > 0)
                throw new ValidationException("FAC-002", errors);
        }
    }
}