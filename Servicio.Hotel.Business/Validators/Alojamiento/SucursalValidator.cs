using System;
using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Alojamiento
{
    public static class SucursalValidator
    {
        private static readonly HashSet<string> TiposAlojamientoPermitidos = new(StringComparer.OrdinalIgnoreCase)
        {
            "hotel", "hostal", "apartamento", "resort", "villa", "cabana", "hostel"
        };

        private static readonly HashSet<string> CategoriasPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            "playa", "ciudad", "montana", "aventura", "cultural", "bienestar"
        };

        public static void ValidateCreate(SucursalCreateDTO dto)
        {
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.CodigoSucursal)) missing.Add(nameof(dto.CodigoSucursal));
            if (string.IsNullOrWhiteSpace(dto.NombreSucursal)) missing.Add(nameof(dto.NombreSucursal));
            if (string.IsNullOrWhiteSpace(dto.TipoAlojamiento)) missing.Add(nameof(dto.TipoAlojamiento));
            if (string.IsNullOrWhiteSpace(dto.Pais)) missing.Add(nameof(dto.Pais));
            if (string.IsNullOrWhiteSpace(dto.Ciudad)) missing.Add(nameof(dto.Ciudad));
            if (string.IsNullOrWhiteSpace(dto.Direccion)) missing.Add(nameof(dto.Direccion));
            if (string.IsNullOrWhiteSpace(dto.Telefono)) missing.Add(nameof(dto.Telefono));
            if (string.IsNullOrWhiteSpace(dto.Correo)) missing.Add(nameof(dto.Correo));

            if (missing.Count > 0)
            {
                var errors = missing.ToDictionary(x => x, _ => new[] { "El campo es requerido." });
                throw new ValidationException("SUC-VAL-001", "Faltan campos obligatorios en la sucursal.", errors);
            }

            if (!TiposAlojamientoPermitidos.Contains(dto.TipoAlojamiento))
                throw new ValidationException("SUC-VAL-002", $"El tipo_alojamiento '{dto.TipoAlojamiento}' no es vÃ¡lido.");

            if (dto.Estrellas.HasValue && (dto.Estrellas.Value < 1 || dto.Estrellas.Value > 5))
                throw new ValidationException("SUC-VAL-003", "El campo estrellas debe estar entre 1 y 5.");

            if (!string.IsNullOrWhiteSpace(dto.CategoriaViaje) && !CategoriasPermitidas.Contains(dto.CategoriaViaje))
                throw new ValidationException("SUC-VAL-004", $"El campo categoria_viaje '{dto.CategoriaViaje}' no es vÃ¡lido.");
        }
        public static void ValidateUpdate(SucursalUpdateDTO dto)
        {
            var missing = new List<string>();

            if (dto.IdSucursal <= 0) missing.Add(nameof(dto.IdSucursal));
            if (string.IsNullOrWhiteSpace(dto.NombreSucursal)) missing.Add(nameof(dto.NombreSucursal));
            if (string.IsNullOrWhiteSpace(dto.TipoAlojamiento)) missing.Add(nameof(dto.TipoAlojamiento));
            if (string.IsNullOrWhiteSpace(dto.Pais)) missing.Add(nameof(dto.Pais));
            if (string.IsNullOrWhiteSpace(dto.Ciudad)) missing.Add(nameof(dto.Ciudad));
            if (string.IsNullOrWhiteSpace(dto.Direccion)) missing.Add(nameof(dto.Direccion));
            if (string.IsNullOrWhiteSpace(dto.Telefono)) missing.Add(nameof(dto.Telefono));
            if (string.IsNullOrWhiteSpace(dto.Correo)) missing.Add(nameof(dto.Correo));

            if (missing.Count > 0)
            {
                var errors = missing.ToDictionary(x => x, _ => new[] { "El campo es requerido." });
                throw new ValidationException("SUC-VAL-005", "Faltan campos obligatorios en la sucursal.", errors);
            }

            if (!TiposAlojamientoPermitidos.Contains(dto.TipoAlojamiento))
                throw new ValidationException("SUC-VAL-006", $"El tipo_alojamiento '{dto.TipoAlojamiento}' no es valido.");

            if (dto.Estrellas.HasValue && (dto.Estrellas.Value < 1 || dto.Estrellas.Value > 5))
                throw new ValidationException("SUC-VAL-007", "El campo estrellas debe estar entre 1 y 5.");

            if (!string.IsNullOrWhiteSpace(dto.CategoriaViaje) && !CategoriasPermitidas.Contains(dto.CategoriaViaje))
                throw new ValidationException("SUC-VAL-008", $"El campo categoria_viaje '{dto.CategoriaViaje}' no es valido.");
        }
    }
}
