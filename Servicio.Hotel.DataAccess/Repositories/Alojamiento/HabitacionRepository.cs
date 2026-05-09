using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Alojamiento
{
    public class HabitacionRepository : RepositoryBase<HabitacionEntity>, IHabitacionRepository
    {
        public HabitacionRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<HabitacionEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<HabitacionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(h => h.HabitacionGuid == guid, ct);

        public async Task<IEnumerable<HabitacionEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<HabitacionEntity> AddAsync(HabitacionEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(HabitacionEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task UpdateEstadoAsync(int id, string nuevoEstado, string usuario, CancellationToken ct = default)
        {
            var habitacion = await GetByIdAsync(id, ct);
            if (habitacion != null)
            {
                habitacion.EstadoHabitacion = nuevoEstado;
                habitacion.ModificadoPorUsuario = usuario;
                habitacion.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(habitacion, ct);
            }
        }

        public async Task<bool> ExistsByNumeroEnSucursalAsync(int idSucursal, string numero, CancellationToken ct = default)
            => await _dbSet.AnyAsync(h => h.IdSucursal == idSucursal && h.NumeroHabitacion == numero, ct);

        public async Task<IEnumerable<HabitacionEntity>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default)
            => await _dbSet.Where(h => h.IdSucursal == idSucursal).ToListAsync(ct);

        public async Task<IEnumerable<HabitacionEntity>> GetByTipoHabitacionAsync(int idTipoHabitacion, CancellationToken ct = default)
            => await _dbSet.Where(h => h.IdTipoHabitacion == idTipoHabitacion).ToListAsync(ct);

        public async Task<IEnumerable<HabitacionEntity>> GetDisponiblesAsync(int idSucursal, DateTime inicio, DateTime fin, CancellationToken ct = default)
        {
            // Habitaciones de la sucursal que estén en estado DIS (Disponible) y no eliminadas
            var habitacionesSucursal = _dbSet.Where(h => h.IdSucursal == idSucursal && h.EstadoHabitacion == "DIS" && !h.EsEliminado);

            // Habitaciones que tienen al menos una reserva que se solapa y NO está cancelada/anulada
            var habitacionesOcupadasIds = await _context.ReservasHabitaciones
                .Where(rh => rh.Reserva.IdSucursal == idSucursal &&
                             rh.Reserva.EstadoReserva != "CAN" && 
                             rh.Reserva.EstadoReserva != "ANU" &&
                             rh.FechaInicio.Date < fin.Date && 
                             rh.FechaFin.Date > inicio.Date)
                .Select(rh => rh.IdHabitacion)
                .Distinct()
                .ToListAsync(ct);

            // Retornar las de la sucursal que NO estén en la lista de ocupadas
            return await habitacionesSucursal
                .Where(h => !habitacionesOcupadasIds.Contains(h.IdHabitacion))
                .ToListAsync(ct);
        }
    }
}