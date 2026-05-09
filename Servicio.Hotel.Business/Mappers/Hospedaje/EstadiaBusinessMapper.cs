using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Hospedaje;
using Servicio.Hotel.DataManagement.Hospedaje.Models;

namespace Servicio.Hotel.Business.Mappers.Hospedaje
{
    public static class EstadiaBusinessMapper
    {
        public static EstadiaDTO ToDto(this EstadiaDataModel model)
        {
            if (model == null) return null;

            return new EstadiaDTO
            {
                IdEstadia = model.IdEstadia,
                EstadiaGuid = model.EstadiaGuid,
                IdReservaHabitacion = model.IdReservaHabitacion,
                IdCliente = model.IdCliente,
                IdHabitacion = model.IdHabitacion,
                CheckinUtc = model.CheckinUtc,
                CheckoutUtc = model.CheckoutUtc,
                EstadoEstadia = model.EstadoEstadia,
                ObservacionesCheckin = model.ObservacionesCheckin,
                ObservacionesCheckout = model.ObservacionesCheckout,
                RequiereMantenimiento = model.RequiereMantenimiento,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion,
                Cargos = model.Cargos?.Select(c => c.ToDto()).ToList()
            };
        }

        public static EstadiaDataModel ToDataModel(this EstadiaDTO dto)
        {
            if (dto == null) return null;

            return new EstadiaDataModel
            {
                IdEstadia = dto.IdEstadia,
                EstadiaGuid = dto.EstadiaGuid,
                IdReservaHabitacion = dto.IdReservaHabitacion,
                IdCliente = dto.IdCliente,
                IdHabitacion = dto.IdHabitacion,
                CheckinUtc = dto.CheckinUtc,
                CheckoutUtc = dto.CheckoutUtc,
                EstadoEstadia = dto.EstadoEstadia,
                ObservacionesCheckin = dto.ObservacionesCheckin,
                ObservacionesCheckout = dto.ObservacionesCheckout,
                RequiereMantenimiento = dto.RequiereMantenimiento,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion,
                Cargos = dto.Cargos?.Select(c => c.ToDataModel()).ToList()
            };
        }

        public static List<EstadiaDTO> ToDtoList(this IEnumerable<EstadiaDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<EstadiaDataModel> ToDataModelList(this IEnumerable<EstadiaDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        // Mapeo para CargoEstadia
        public static CargoEstadiaDTO ToDto(this CargoEstadiaDataModel model)
        {
            if (model == null) return null;

            return new CargoEstadiaDTO
            {
                IdCargoEstadia = model.IdCargoEstadia,
                CargoGuid = model.CargoGuid,
                IdEstadia = model.IdEstadia,
                IdCatalogo = model.IdCatalogo,
                DescripcionCargo = model.DescripcionCargo,
                Cantidad = model.Cantidad,
                PrecioUnitario = model.PrecioUnitario,
                Subtotal = model.Subtotal,
                ValorIva = model.ValorIva,
                TotalCargo = model.TotalCargo,
                FechaConsumoUtc = model.FechaConsumoUtc,
                EstadoCargo = model.EstadoCargo,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static CargoEstadiaDataModel ToDataModel(this CargoEstadiaDTO dto)
        {
            if (dto == null) return null;

            return new CargoEstadiaDataModel
            {
                IdCargoEstadia = dto.IdCargoEstadia,
                CargoGuid = dto.CargoGuid,
                IdEstadia = dto.IdEstadia,
                IdCatalogo = dto.IdCatalogo,
                DescripcionCargo = dto.DescripcionCargo,
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario,
                Subtotal = dto.Subtotal,
                ValorIva = dto.ValorIva,
                TotalCargo = dto.TotalCargo,
                FechaConsumoUtc = dto.FechaConsumoUtc,
                EstadoCargo = dto.EstadoCargo,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion
            };
        }

        public static List<CargoEstadiaDTO> ToDtoList(this IEnumerable<CargoEstadiaDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<CargoEstadiaDataModel> ToDataModelList(this IEnumerable<CargoEstadiaDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static EstadiaFiltroDataModel ToDataModel(this EstadiaFiltroDTO dto)
        {
            if (dto == null) return null;

            return new EstadiaFiltroDataModel
            {
                IdReservaHabitacion = dto.IdReservaHabitacion,
                IdCliente = dto.IdCliente,
                IdHabitacion = dto.IdHabitacion,
                EstadoEstadia = dto.EstadoEstadia,
                CheckinDesde = dto.CheckinDesde,
                CheckinHasta = dto.CheckinHasta,
                RequiereMantenimiento = dto.RequiereMantenimiento
            };
        }
    }
}