using System;
using System.Collections.Generic;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Alojamiento
{
    public static class HabitacionValidator
    {
        public static void Validate(HabitacionDTO habitacion)
        {
            if (habitacion == null)
                throw new ValidationException("HAB-001", "La habitación no puede ser nula.");

            var errors = new Dictionary<string, string[]>();

            if (habitacion.IdSucursal <= 0)
                errors["IdSucursal"] = new[] { "El id de sucursal debe ser mayor a cero." };

            if (habitacion.IdTipoHabitacion <= 0)
                errors["IdTipoHabitacion"] = new[] { "El id de tipo de habitación debe ser mayor a cero." };

            if (string.IsNullOrWhiteSpace(habitacion.NumeroHabitacion))
                errors["NumeroHabitacion"] = new[] { "El número de habitación es obligatorio." };
            else if (habitacion.NumeroHabitacion.Length > 20)
                errors["NumeroHabitacion"] = new[] { "El número de habitación no puede exceder 20 caracteres." };

            if (habitacion.CapacidadHabitacion <= 0)
                errors["CapacidadHabitacion"] = new[] { "La capacidad de la habitación debe ser mayor a cero." };

            if (habitacion.PrecioBase <= 0)
                errors["PrecioBase"] = new[] { "El precio base debe ser mayor a cero." };

            var estadosValidos = new[] { "DIS", "OCU", "MNT", "FDS", "INA" };
            if (!string.IsNullOrWhiteSpace(habitacion.EstadoHabitacion) &&
                !estadosValidos.Contains(habitacion.EstadoHabitacion))
                errors["EstadoHabitacion"] = new[] { $"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}." };

            if (errors.Count > 0)
                throw new ValidationException("HAB-002", errors);
        }
    }
}