using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Hospedaje;

namespace Servicio.Hotel.DataAccess.Repositories.Hospedaje
{
    public class EstadiaRepository : RepositoryBase<EstadiaEntity>, IEstadiaRepository
    {
        public EstadiaRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<EstadiaEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<EstadiaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(e => e.EstadiaGuid == guid, ct);

        public async Task<IEnumerable<EstadiaEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<EstadiaEntity> AddAsync(EstadiaEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(EstadiaEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default)
        {
            var estadia = await GetByIdAsync(idEstadia, ct);
            if (estadia != null)
            {
                estadia.CheckoutUtc = DateTime.UtcNow;
                estadia.ObservacionesCheckout = observaciones;
                estadia.RequiereMantenimiento = requiereMantenimiento;
                estadia.EstadoEstadia = "FIN";
                estadia.ModificadoPorUsuario = usuario;
                estadia.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(estadia, ct);
            }
        }

        public async Task<bool> EstaFinalizadaAsync(int idEstadia, CancellationToken ct = default)
        {
            var estadia = await GetByIdAsync(idEstadia, ct);
            return estadia != null && estadia.EstadoEstadia == "FIN";
        }
        public async Task<int> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            // IMPORTANT: pass CancellationToken via the proper overload.
            // Otherwise `ct` is treated as a SQL parameter value and EF tries to map it (fails for CancellationToken).
            return await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC booking.SP_HACER_CHECKIN @id_reserva = {idReserva}, @usuario = {usuario}",
                ct);
        }

        public async Task<IEnumerable<EstadiaEntity>> GetByReservaAsync(int idReserva, CancellationToken ct = default)
        {
            return await (from e in _context.Estadias
                          join rh in _context.ReservasHabitaciones on e.IdReservaHabitacion equals rh.IdReservaHabitacion
                          where rh.IdReserva == idReserva
                          select e).ToListAsync(ct);
        }
    }
}
