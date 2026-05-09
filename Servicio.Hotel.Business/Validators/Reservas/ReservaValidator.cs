using System;
using System.Collections.Generic;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Reservas
{
    public static class ReservaValidator
    {
        public static void Validate(ReservaDTO reserva)
        {
            if (reserva == null)
                throw new ValidationException("RES-001", "La reserva no puede ser nula.");

            var errors = new Dictionary<string, string[]>();

            if (reserva.IdCliente <= 0)
                errors["IdCliente"] = new[] { "El id del cliente es obligatorio." };

            if (reserva.IdSucursal <= 0)
                errors["IdSucursal"] = new[] { "El id de la sucursal es obligatorio." };

            if (reserva.FechaFin <= reserva.FechaInicio)
                errors["FechaFin"] = new[] { "La fecha de fin debe ser posterior a la fecha de inicio." };

            if (string.IsNullOrWhiteSpace(reserva.OrigenCanalReserva))
                errors["OrigenCanalReserva"] = new[] { "El canal de origen es obligatorio." };

            var estadosValidos = new[] { "PEN", "CON", "CAN", "EXP", "FIN", "EMI" };
            if (!string.IsNullOrWhiteSpace(reserva.EstadoReserva) &&
                !estadosValidos.Contains(reserva.EstadoReserva))
                errors["EstadoReserva"] = new[] { $"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}." };

            if (errors.Count > 0)
                throw new ValidationException("RES-002", errors);
        }
    }
}