using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Mappers.Alojamiento;
using Servicio.Hotel.Business.Validators.Alojamiento;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Services.Alojamiento
{
    public class SucursalService : ISucursalService
    {
        private readonly ISucursalDataService _dataService;
        private readonly ServicioHotelDbContext _context;

        public SucursalService(ISucursalDataService dataService, ServicioHotelDbContext context)
        {
            _dataService = dataService;
            _context = context;
        }

        public async Task<IEnumerable<SucursalDTO>> GetAllAsync(string? estado = null, CancellationToken ct = default)
        {
            var all = (await _dataService.GetAllAsync(ct)).ToDtoList();

            if (!string.IsNullOrWhiteSpace(estado))
                return all.Where(s => s.EstadoSucursal == estado).ToList();

            return all.Where(s => s.EstadoSucursal != "INA").ToList();
        }

        public async Task<SucursalDTO> GetByIdAsync(int id, CancellationToken ct = default)
            => (await _dataService.GetByIdAsync(id, ct)).ToDto()
               ?? throw new NotFoundException("SUC-001", $"No se encontró la sucursal con ID {id}.");

        public async Task<SucursalDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => (await _dataService.GetByGuidAsync(guid, ct)).ToDto()
               ?? throw new NotFoundException("SUC-001", $"No se encontrÃ³ la sucursal con GUID {guid}.");

        public async Task<SucursalDTO> CreateAsync(SucursalCreateDTO dto, CancellationToken ct = default)
        {
            SucursalValidator.ValidateCreate(dto);
            await EnsureNombreUnicoAsync(dto.NombreSucursal, null, ct);
            var created = await _dataService.AddAsync(dto.ToDataModel()!, ct);
            await ReplaceImagenesAsync(created.IdSucursal, dto.Imagenes, ct);
            return (await _dataService.GetByIdAsync(created.IdSucursal, ct)).ToDto()!;
        }

        public async Task UpdateAsync(SucursalUpdateDTO dto, CancellationToken ct = default)
        {
            _ = await GetByIdAsync(dto.IdSucursal, ct);
            SucursalValidator.ValidateUpdate(dto);
            await EnsureNombreUnicoAsync(dto.NombreSucursal, dto.IdSucursal, ct);
            await _dataService.UpdateAsync(dto.ToDataModel()!, ct);
            await ReplaceImagenesAsync(dto.IdSucursal, dto.Imagenes, ct);
        }

        public async Task UpdatePoliticasAsync(Guid sucursalGuid, SucursalPoliticasUpdateDTO dto, string usuario, CancellationToken ct = default)
        {
            var existing = await _dataService.GetByGuidAsync(sucursalGuid, ct);
            if (existing == null)
                throw new NotFoundException("SUC-001", $"No se encontrÃ³ la sucursal con GUID {sucursalGuid}.");

            var politicas = new SucursalDataModel
            {
                HoraCheckin = dto.HoraCheckin,
                HoraCheckout = dto.HoraCheckout,
                PermiteMascotas = dto.PermiteMascotas,
                SePermiteFumar = dto.SePermiteFumar,
                AceptaNinos = dto.AceptaNinos,
                CheckinAnticipado = dto.CheckinAnticipado,
                CheckoutTardio = dto.CheckoutTardio,
                ModificadoPorUsuario = usuario
            };

            await _dataService.UpdatePoliticasAsync(existing.IdSucursal, politicas, ct);
        }

        public async Task InhabilitarAsync(Guid sucursalGuid, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _dataService.GetByGuidAsync(sucursalGuid, ct);
            if (existing == null)
                throw new NotFoundException("SUC-001", $"No se encontrÃ³ la sucursal con GUID {sucursalGuid}.");

            await _dataService.InhabilitarAsync(existing.IdSucursal, motivo, usuario, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            _ = await GetByIdAsync(id, ct);
            await EnsureSinHabitacionesActivasAsync(id, ct);
            await _dataService.DeleteAsync(id, ct);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
        {
            var existing = await _dataService.GetByGuidAsync(guid, ct);
            if (existing == null)
                throw new NotFoundException("SUC-001", $"No se encontrÃ³ la sucursal con GUID {guid}.");

            await EnsureSinHabitacionesActivasAsync(existing.IdSucursal, ct);
            await _dataService.DeleteAsync(existing.IdSucursal, ct);
        }

        private async Task EnsureNombreUnicoAsync(string nombreSucursal, int? idSucursal, CancellationToken ct)
        {
            var normalized = (nombreSucursal ?? string.Empty).Trim().ToUpperInvariant();
            var exists = await _context.Sucursales.AnyAsync(s =>
                s.NombreSucursal.Trim().ToUpper() == normalized &&
                (!idSucursal.HasValue || s.IdSucursal != idSucursal.Value), ct);

            if (exists)
                throw new ConflictException("Ya existe una sucursal registrada con ese nombre.");
        }

        private async Task EnsureSinHabitacionesActivasAsync(int idSucursal, CancellationToken ct)
        {
            var tieneHabitacionesActivas = await _context.Habitaciones.AnyAsync(h =>
                h.IdSucursal == idSucursal &&
                h.EstadoHabitacion != "INA", ct);

            if (tieneHabitacionesActivas)
                throw new ConflictException("No se puede eliminar la sucursal porque tiene habitaciones activas asociadas.");
        }

        private async Task ReplaceImagenesAsync(int idSucursal, List<ImagenDTO>? imagenes, CancellationToken ct)
        {
            if (imagenes == null)
                return;

            var existing = await _context.SucursalImagenes
                .Where(i => i.IdSucursal == idSucursal)
                .ToListAsync(ct);

            foreach (var image in existing)
            {
                image.Estado = "INA";
                image.FechaModificacionUtc = DateTime.UtcNow;
            }

            var clean = imagenes
                .Where(i => !string.IsNullOrWhiteSpace(i.UrlImagen))
                .OrderBy(i => i.Orden <= 0 ? int.MaxValue : i.Orden)
                .ToList();

            for (var index = 0; index < clean.Count; index++)
            {
                var image = clean[index];
                _context.SucursalImagenes.Add(new SucursalImagenEntity
                {
                    GuidSucursalImagen = image.ImagenGuid == Guid.Empty ? Guid.NewGuid() : image.ImagenGuid,
                    IdSucursal = idSucursal,
                    UrlImagen = image.UrlImagen.Trim(),
                    Descripcion = image.Descripcion,
                    Orden = image.Orden > 0 ? image.Orden : index + 1,
                    Estado = "ACT",
                    FechaCreacionUtc = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(ct);
        }
    }
}
