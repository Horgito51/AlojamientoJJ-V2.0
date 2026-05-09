using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Alojamiento
{
    public class SucursalRepository : RepositoryBase<SucursalEntity>, ISucursalRepository
    {
        public SucursalRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<SucursalEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<SucursalEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(s => s.SucursalGuid == guid, ct);

        public async Task<IEnumerable<SucursalEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<SucursalEntity> AddAsync(SucursalEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(SucursalEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<SucursalEntity?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(s => s.CodigoSucursal == codigo, ct);

        public async Task UpdatePoliticasAsync(int id, SucursalEntity politicas, CancellationToken ct = default)
        {
            var sucursal = await GetByIdAsync(id, ct);
            if (sucursal != null)
            {
                sucursal.HoraCheckin = politicas.HoraCheckin;
                sucursal.HoraCheckout = politicas.HoraCheckout;
                sucursal.CheckinAnticipado = politicas.CheckinAnticipado;
                sucursal.CheckoutTardio = politicas.CheckoutTardio;
                sucursal.AceptaNinos = politicas.AceptaNinos;
                sucursal.EdadMinimaHuesped = politicas.EdadMinimaHuesped;
                sucursal.PermiteMascotas = politicas.PermiteMascotas;
                sucursal.SePermiteFumar = politicas.SePermiteFumar;
                sucursal.ModificadoPorUsuario = politicas.ModificadoPorUsuario;
                sucursal.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(sucursal, ct);
            }
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var sucursal = await GetByIdAsync(id, ct);
            if (sucursal != null)
            {
                sucursal.EstadoSucursal = "INA";
                sucursal.FechaInhabilitacionUtc = DateTime.UtcNow;
                sucursal.MotivoInhabilitacion = motivo;
                sucursal.ModificadoPorUsuario = usuario;
                sucursal.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(sucursal, ct);
            }
        }
    }
}