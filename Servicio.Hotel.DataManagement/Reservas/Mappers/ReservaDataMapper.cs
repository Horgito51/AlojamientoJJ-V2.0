using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataManagement.Reservas.Models;

namespace Servicio.Hotel.DataManagement.Reservas.Mappers
{
    public static class ReservaDataMapper
    {
        public static ReservaDataModel ToModel(this ReservaEntity entity)
        {
            if (entity == null) return null;

            return new ReservaDataModel
            {
                IdReserva = entity.IdReserva,
                GuidReserva = entity.GuidReserva,
                CodigoReserva = entity.CodigoReserva,
                IdCliente = entity.IdCliente,
                IdSucursal = entity.IdSucursal,
                FechaReservaUtc = entity.FechaReservaUtc,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                SubtotalReserva = entity.SubtotalReserva,
                ValorIva = entity.ValorIva,
                TotalReserva = entity.TotalReserva,
                DescuentoAplicado = entity.DescuentoAplicado,
                SaldoPendiente = entity.SaldoPendiente,
                OrigenCanalReserva = entity.OrigenCanalReserva,
                EstadoReserva = entity.EstadoReserva,
                FechaConfirmacionUtc = entity.FechaConfirmacionUtc,
                FechaCancelacionUtc = entity.FechaCancelacionUtc,
                MotivoCancelacion = entity.MotivoCancelacion,
                Observaciones = entity.Observaciones,
                EsWalkin = entity.EsWalkin,
                EsEliminado = entity.EsEliminado,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                RowVersion = entity.RowVersion,
                Habitaciones = entity.ReservasHabitaciones?.Select(rh => rh.ToModel()).ToList()
            };
        }

        public static ReservaEntity ToEntity(this ReservaDataModel model)
        {
            if (model == null) return null;

            return new ReservaEntity
            {
                IdReserva = model.IdReserva,
                GuidReserva = model.GuidReserva,
                CodigoReserva = model.CodigoReserva,
                IdCliente = model.IdCliente,
                IdSucursal = model.IdSucursal,
                FechaReservaUtc = model.FechaReservaUtc,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                SubtotalReserva = model.SubtotalReserva,
                ValorIva = model.ValorIva,
                TotalReserva = model.TotalReserva,
                DescuentoAplicado = model.DescuentoAplicado,
                SaldoPendiente = model.SaldoPendiente,
                OrigenCanalReserva = model.OrigenCanalReserva,
                EstadoReserva = model.EstadoReserva,
                FechaConfirmacionUtc = model.FechaConfirmacionUtc,
                FechaCancelacionUtc = model.FechaCancelacionUtc,
                MotivoCancelacion = model.MotivoCancelacion,
                Observaciones = model.Observaciones,
                EsWalkin = model.EsWalkin,
                EsEliminado = model.EsEliminado,
                CreadoPorUsuario = model.CreadoPorUsuario,
                FechaRegistroUtc = model.FechaRegistroUtc,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                RowVersion = model.RowVersion,
                ReservasHabitaciones = model.Habitaciones?.Select(rh => rh.ToEntity()).ToList()
            };
        }

        public static ReservaHabitacionDataModel ToModel(this ReservaHabitacionEntity entity)
        {
            if (entity == null) return null;

            return new ReservaHabitacionDataModel
            {
                IdReservaHabitacion = entity.IdReservaHabitacion,
                ReservaHabitacionGuid = entity.ReservaHabitacionGuid,
                IdReserva = entity.IdReserva,
                IdHabitacion = entity.IdHabitacion,
                IdTarifa = entity.IdTarifa,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                NumAdultos = entity.NumAdultos,
                NumNinos = entity.NumNinos,
                PrecioNocheAplicado = entity.PrecioNocheAplicado,
                SubtotalLinea = entity.SubtotalLinea,
                ValorIvaLinea = entity.ValorIvaLinea,
                DescuentoLinea = entity.DescuentoLinea,
                TotalLinea = entity.TotalLinea,
                EstadoDetalle = entity.EstadoDetalle,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion
            };
        }

        public static ReservaHabitacionEntity ToEntity(this ReservaHabitacionDataModel model)
        {
            if (model == null) return null;

            return new ReservaHabitacionEntity
            {
                IdReservaHabitacion = model.IdReservaHabitacion,
                ReservaHabitacionGuid = model.ReservaHabitacionGuid,
                IdReserva = model.IdReserva,
                IdHabitacion = model.IdHabitacion,
                IdTarifa = model.IdTarifa,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                NumAdultos = model.NumAdultos,
                NumNinos = model.NumNinos,
                PrecioNocheAplicado = model.PrecioNocheAplicado,
                SubtotalLinea = model.SubtotalLinea,
                ValorIvaLinea = model.ValorIvaLinea,
                DescuentoLinea = model.DescuentoLinea,
                TotalLinea = model.TotalLinea,
                EstadoDetalle = model.EstadoDetalle,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static List<ReservaDataModel> ToModelList(this IEnumerable<ReservaEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<ReservaEntity> ToEntityList(this IEnumerable<ReservaDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}