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
    public class TarifaRepository : RepositoryBase<TarifaEntity>, ITarifaRepository
    {
        public TarifaRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<TarifaEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<TarifaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(t => t.TarifaGuid == guid, ct);

        public async Task<IEnumerable<TarifaEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<TarifaEntity> AddAsync(TarifaEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(TarifaEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task DesactivarAsync(int id, string usuario, CancellationToken ct = default)
        {
            var tarifa = await GetByIdAsync(id, ct);
            if (tarifa != null)
            {
                tarifa.EstadoTarifa = "INA";
                tarifa.ModificadoPorUsuario = usuario;
                tarifa.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(tarifa, ct);
            }
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default)
            => await _dbSet.AnyAsync(t => t.CodigoTarifa == codigo, ct);

        // Dentro de la clase TarifaRepository, agregar:

        public async Task<TarifaEntity?> GetTarifaVigenteAsync(int idSucursal, int idTipoHabitacion, DateTime fecha, string? canal = null, CancellationToken ct = default)
        {
            var canalNormalizado = NormalizarCanal(canal);
            return await _dbSet
                .Where(t => t.IdSucursal == idSucursal && t.IdTipoHabitacion == idTipoHabitacion
                    && t.EstadoTarifa == "ACT" && t.FechaInicio <= fecha && t.FechaFin >= fecha
                    && (t.CanalTarifa.ToUpper() == "TODOS" || t.CanalTarifa.ToUpper() == canalNormalizado))
                .OrderByDescending(t => t.Prioridad)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<TarifaEntity?> GetTarifaVigenteRangoAsync(int idSucursal, int idTipoHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default)
        {
            // fechaFin es checkout (exclusivo): las noches aplican hasta fechaFin - 1 día
            var start = fechaInicio.Date;
            var endInclusive = fechaFin.Date.AddDays(-1);
            var canalNormalizado = NormalizarCanal(canal);

            return await _dbSet
                .Where(t => t.IdSucursal == idSucursal
                            && t.IdTipoHabitacion == idTipoHabitacion
                            && t.EstadoTarifa == "ACT"
                            && t.FechaInicio.Date <= start
                            && t.FechaFin.Date >= endInclusive
                            && (t.CanalTarifa.ToUpper() == "TODOS" || t.CanalTarifa.ToUpper() == canalNormalizado))
                .OrderByDescending(t => t.Prioridad)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<TarifaEntity>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default)
        {
            return await _dbSet.Where(t => t.IdSucursal == idSucursal).ToListAsync(ct);
        }

        private static string NormalizarCanal(string? canal)
            => string.IsNullOrWhiteSpace(canal) ? "TODOS" : canal.Trim().ToUpperInvariant();
    }
}
