using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Valoraciones;

namespace Servicio.Hotel.DataAccess.Repositories.Valoraciones
{
    public class ValoracionRepository : RepositoryBase<ValoracionEntity>, IValoracionRepository
    {
        public ValoracionRepository(ServicioHotelDbContext context) : base(context)
        {
        }

        public async Task<ValoracionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.ValoracionGuid == guid, ct);
        }

        public async Task ModerarAsync(int idValoracion, string nuevoEstado, string motivo, string moderador, CancellationToken ct = default)
        {
            var valoracion = await GetByIdAsync(idValoracion, ct);
            if (valoracion != null)
            {
                valoracion.EstadoValoracion = nuevoEstado;
                valoracion.PublicadaEnPortal = nuevoEstado == "PUB";
                valoracion.ModeradaPorUsuario = moderador;
                valoracion.MotivoModeracion = motivo;
                valoracion.ModificadoPorUsuario = moderador;
                valoracion.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(valoracion, ct);
            }
        }

        public async Task ResponderAsync(int idValoracion, string respuesta, string usuario, CancellationToken ct = default)
        {
            var valoracion = await GetByIdAsync(idValoracion, ct);
            if (valoracion != null)
            {
                valoracion.RespuestaHotel = respuesta;
                valoracion.FechaRespuestaUtc = DateTime.UtcNow;
                valoracion.ModificadoPorUsuario = usuario;
                valoracion.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(valoracion, ct);
            }
        }

        public async Task<bool> ExistsByEstadiaAsync(int idEstadia, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(v => v.IdEstadia == idEstadia, ct);
        }
    }
}