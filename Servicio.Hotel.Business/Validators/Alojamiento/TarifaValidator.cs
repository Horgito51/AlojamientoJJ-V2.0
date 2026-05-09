using System;
using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Alojamiento
{
    public static class TarifaValidator
    {
        public static void Validate(TarifaDTO tarifa)
        {
            if (tarifa == null)
                throw new ValidationException("TAR-VAL-001", "La tarifa no puede ser nula.");

            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(tarifa.CodigoTarifa))
                errors["CodigoTarifa"] = new[] { "El código de tarifa es obligatorio." };
            else if (tarifa.CodigoTarifa.Length > 30)
                errors["CodigoTarifa"] = new[] { "El código de tarifa no puede exceder 30 caracteres." };

            if (tarifa.IdSucursal <= 0)
                errors["IdSucursal"] = new[] { "El id de sucursal debe ser mayor a cero." };

            if (tarifa.IdTipoHabitacion <= 0)
                errors["IdTipoHabitacion"] = new[] { "El id de tipo de habitación debe ser mayor a cero." };

            if (string.IsNullOrWhiteSpace(tarifa.NombreTarifa))
                errors["NombreTarifa"] = new[] { "El nombre de la tarifa es obligatorio." };
            else if (tarifa.NombreTarifa.Length > 150)
                errors["NombreTarifa"] = new[] { "El nombre de la tarifa no puede exceder 150 caracteres." };

            if (string.IsNullOrWhiteSpace(tarifa.CanalTarifa))
                errors["CanalTarifa"] = new[] { "El canal de la tarifa es obligatorio." };
            else if (tarifa.CanalTarifa.Length > 30)
                errors["CanalTarifa"] = new[] { "El canal de la tarifa no puede exceder 30 caracteres." };

            if (tarifa.FechaInicio == default)
                errors["FechaInicio"] = new[] { "La fecha de inicio es obligatoria." };

            if (tarifa.FechaFin == default)
                errors["FechaFin"] = new[] { "La fecha de fin es obligatoria." };
            else if (tarifa.FechaInicio != default && tarifa.FechaFin < tarifa.FechaInicio)
                errors["FechaFin"] = new[] { "La fecha de fin debe ser mayor o igual a la fecha de inicio." };

            if (tarifa.PrecioPorNoche <= 0)
                errors["PrecioPorNoche"] = new[] { "El precio por noche debe ser mayor a cero." };

            if (tarifa.PorcentajeIva < 0)
                errors["PorcentajeIva"] = new[] { "El porcentaje de IVA no puede ser negativo." };

            if (tarifa.MinNoches <= 0)
                errors["MinNoches"] = new[] { "El mínimo de noches debe ser mayor a cero." };

            if (tarifa.MaxNoches.HasValue && tarifa.MaxNoches.Value <= 0)
                errors["MaxNoches"] = new[] { "El máximo de noches, si se especifica, debe ser mayor a cero." };
            else if (tarifa.MaxNoches.HasValue && tarifa.MinNoches > 0 && tarifa.MaxNoches.Value < tarifa.MinNoches)
                errors["MaxNoches"] = new[] { "El máximo de noches no puede ser menor al mínimo de noches." };

            if (tarifa.Prioridad < 0)
                errors["Prioridad"] = new[] { "La prioridad no puede ser negativa." };

            var estadosValidos = new[] { "ACT", "INA" };
            if (!string.IsNullOrWhiteSpace(tarifa.EstadoTarifa) &&
                !estadosValidos.Contains(tarifa.EstadoTarifa))
                errors["EstadoTarifa"] = new[] { $"Estado inválido. Valores permitidos: {string.Join(", ", estadosValidos)}." };

            if (errors.Count > 0)
                throw new ValidationException("TAR-VAL-002", errors);
        }
    }
}

