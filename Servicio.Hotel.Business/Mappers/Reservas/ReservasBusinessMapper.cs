using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.DataManagement.Reservas.Models;

namespace Servicio.Hotel.Business.Mappers.Reservas
{
    public static class ReservaBusinessMapper
    {
        // Mapeo de ReservaDataModel -> ReservaDTO
        public static ReservaDTO ToDto(this ReservaDataModel model)
        {
            if (model == null) return null;

            return new ReservaDTO
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
                Habitaciones = model.Habitaciones?.Select(h => h.ToDto()).ToList()
            };
        }

        // Mapeo de ReservaDTO -> ReservaDataModel
        public static ReservaDataModel ToDataModel(this ReservaDTO dto)
        {
            if (dto == null) return null;

            return new ReservaDataModel
            {
                IdReserva = dto.IdReserva,
                GuidReserva = dto.GuidReserva,
                CodigoReserva = dto.CodigoReserva,
                IdCliente = dto.IdCliente,
                IdSucursal = dto.IdSucursal,
                FechaReservaUtc = dto.FechaReservaUtc,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                SubtotalReserva = dto.SubtotalReserva,
                ValorIva = dto.ValorIva,
                TotalReserva = dto.TotalReserva,
                DescuentoAplicado = dto.DescuentoAplicado,
                SaldoPendiente = dto.SaldoPendiente,
                OrigenCanalReserva = dto.OrigenCanalReserva,
                EstadoReserva = dto.EstadoReserva,
                FechaConfirmacionUtc = dto.FechaConfirmacionUtc,
                FechaCancelacionUtc = dto.FechaCancelacionUtc,
                MotivoCancelacion = dto.MotivoCancelacion,
                Observaciones = dto.Observaciones,
                EsWalkin = dto.EsWalkin,
                EsEliminado = dto.EsEliminado,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                RowVersion = dto.RowVersion,
                Habitaciones = dto.Habitaciones?.Select(h => h.ToDataModel()).ToList()
            };
        }

        // Mapeo de listas
        public static List<ReservaDTO> ToDtoList(this IEnumerable<ReservaDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<ReservaDataModel> ToDataModelList(this IEnumerable<ReservaDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        // ---------- Mapeo para ReservaHabitacion ----------
        public static ReservaHabitacionDTO ToDto(this ReservaHabitacionDataModel model)
        {
            if (model == null) return null;

            return new ReservaHabitacionDTO
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

        public static ReservaHabitacionDataModel ToDataModel(this ReservaHabitacionDTO dto)
        {
            if (dto == null) return null;

            return new ReservaHabitacionDataModel
            {
                IdReservaHabitacion = dto.IdReservaHabitacion,
                ReservaHabitacionGuid = dto.ReservaHabitacionGuid,
                IdReserva = dto.IdReserva,
                IdHabitacion = dto.IdHabitacion,
                IdTarifa = dto.IdTarifa,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                NumAdultos = dto.NumAdultos,
                NumNinos = dto.NumNinos,
                PrecioNocheAplicado = dto.PrecioNocheAplicado,
                SubtotalLinea = dto.SubtotalLinea,
                ValorIvaLinea = dto.ValorIvaLinea,
                DescuentoLinea = dto.DescuentoLinea,
                TotalLinea = dto.TotalLinea,
                EstadoDetalle = dto.EstadoDetalle,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion
            };
        }

        public static List<ReservaHabitacionDTO> ToDtoList(this IEnumerable<ReservaHabitacionDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<ReservaHabitacionDataModel> ToDataModelList(this IEnumerable<ReservaHabitacionDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static ReservaFiltroDataModel ToDataModel(this ReservaFiltroDTO dto)
        {
            if (dto == null) return null;

            return new ReservaFiltroDataModel
            {
                IdCliente = dto.IdCliente,
                IdSucursal = dto.IdSucursal,
                EstadoReserva = dto.EstadoReserva,
                FechaInicioDesde = dto.FechaInicioDesde,
                FechaInicioHasta = dto.FechaInicioHasta,
                CodigoReserva = dto.CodigoReserva,
                EsWalkin = dto.EsWalkin,
                EsEliminado = dto.EsEliminado
            };
        }
    }
}