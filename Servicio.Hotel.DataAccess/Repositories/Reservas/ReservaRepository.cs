using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.DataAccess.Repositories.Reservas
{
    public class ReservaRepository : RepositoryBase<ReservaEntity>, IReservaRepository
    {
        public ReservaRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<ReservaEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.Include(r => r.ReservasHabitaciones).FirstOrDefaultAsync(r => r.IdReserva == id, ct);

        public async Task<IEnumerable<ReservaEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<ReservaEntity> AddAsync(ReservaEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(ReservaEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<ReservaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.Include(r => r.ReservasHabitaciones).FirstOrDefaultAsync(r => r.GuidReserva == guid, ct);

        public async Task<ReservaEntity?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
            => await _dbSet.Include(r => r.ReservasHabitaciones).FirstOrDefaultAsync(r => r.CodigoReserva == codigo, ct);

        public async Task ConfirmarAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await GetByIdAsync(idReserva, ct);
            if (reserva != null && reserva.EstadoReserva == "PEN")
            {
                reserva.EstadoReserva = "CON";
                reserva.FechaConfirmacionUtc = DateTime.UtcNow;
                reserva.ModificadoPorUsuario = usuario;
                reserva.FechaModificacionUtc = DateTime.UtcNow;

                if (reserva.ReservasHabitaciones != null)
                {
                    foreach (var detalle in reserva.ReservasHabitaciones)
                    {
                        detalle.EstadoDetalle = "CON";
                        detalle.ModificadoPorUsuario = usuario;
                        detalle.FechaModificacionUtc = DateTime.UtcNow;
                    }
                }
                await UpdateAsync(reserva, ct);
            }
        }

        public async Task CancelarAsync(int idReserva, string motivo, string usuario, CancellationToken ct = default)
        {
            var reserva = await GetByIdAsync(idReserva, ct);
            if (reserva != null && (reserva.EstadoReserva == "PEN" || reserva.EstadoReserva == "CON"))
            {
                reserva.EstadoReserva = "CAN";
                reserva.FechaCancelacionUtc = DateTime.UtcNow;
                reserva.MotivoCancelacion = motivo;
                reserva.ModificadoPorUsuario = usuario;
                reserva.FechaModificacionUtc = DateTime.UtcNow;

                if (reserva.ReservasHabitaciones != null)
                {
                    foreach (var detalle in reserva.ReservasHabitaciones)
                    {
                        detalle.EstadoDetalle = "CAN";
                        detalle.ModificadoPorUsuario = usuario;
                        detalle.FechaModificacionUtc = DateTime.UtcNow;
                    }
                }
                await UpdateAsync(reserva, ct);
            }
        }

        public async Task FinalizarAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await GetByIdAsync(idReserva, ct);
            if (reserva != null && reserva.EstadoReserva == "EMI")
            {
                reserva.EstadoReserva = "FIN";
                reserva.ModificadoPorUsuario = usuario;
                reserva.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(reserva, ct);
            }
        }

        public async Task<bool> PuedeCancelarAsync(int idReserva, CancellationToken ct = default)
        {
            var reserva = await GetByIdAsync(idReserva, ct);
            return reserva != null && (reserva.EstadoReserva == "PEN" || reserva.EstadoReserva == "CON")
                && reserva.FechaInicio > DateTime.UtcNow;
        }
        public async Task<int> ConfirmarReservaHabitacionAsync(int idReserva, int idHabitacion, int? idTarifa, DateTime fechaInicio, DateTime fechaFin, int numAdultos, int numNinos, decimal precioNoche, string usuario, CancellationToken ct = default)
        {
            var sql = "EXEC booking.SP_CONFIRMAR_RESERVA_HABITACION @id_reserva, @id_habitacion, @id_tarifa, @fecha_inicio, @fecha_fin, @num_adultos, @num_ninos, @precio_noche, @usuario";
            var parameters = new[]
            {
        new SqlParameter("@id_reserva", idReserva),
        new SqlParameter("@id_habitacion", idHabitacion),
        new SqlParameter("@id_tarifa", idTarifa ?? (object)DBNull.Value),
        new SqlParameter("@fecha_inicio", fechaInicio),
        new SqlParameter("@fecha_fin", fechaFin),
        new SqlParameter("@num_adultos", numAdultos),
        new SqlParameter("@num_ninos", numNinos),
        new SqlParameter("@precio_noche", precioNoche),
        new SqlParameter("@usuario", usuario)
    };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters, ct);
        }
    }
}
