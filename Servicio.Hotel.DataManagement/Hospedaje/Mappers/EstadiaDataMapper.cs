using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;
using Servicio.Hotel.DataManagement.Hospedaje.Models;

namespace Servicio.Hotel.DataManagement.Hospedaje.Mappers
{
    public static class EstadiaDataMapper
    {
        // Mapeo de EstadiaEntity a EstadiaDataModel
        public static EstadiaDataModel ToModel(this EstadiaEntity entity)
        {
            if (entity == null) return null;

            return new EstadiaDataModel
            {
                IdEstadia = entity.IdEstadia,
                EstadiaGuid = entity.EstadiaGuid,
                IdReservaHabitacion = entity.IdReservaHabitacion,
                IdCliente = entity.IdCliente,
                IdHabitacion = entity.IdHabitacion,
                CheckinUtc = entity.CheckinUtc,
                CheckoutUtc = entity.CheckoutUtc,
                EstadoEstadia = entity.EstadoEstadia,
                ObservacionesCheckin = entity.ObservacionesCheckin,
                ObservacionesCheckout = entity.ObservacionesCheckout,
                RequiereMantenimiento = entity.RequiereMantenimiento,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion,
                Cargos = entity.CargosEstadia?.Select(c => c.ToModel()).ToList()
            };
        }

        // Mapeo de EstadiaDataModel a EstadiaEntity
        public static EstadiaEntity ToEntity(this EstadiaDataModel model)
        {
            if (model == null) return null;

            return new EstadiaEntity
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
                CargosEstadia = model.Cargos?.Select(c => c.ToEntity()).ToList()
            };
        }

        // Mapeo de CargoEstadiaEntity a CargoEstadiaDataModel
        public static CargoEstadiaDataModel ToModel(this CargoEstadiaEntity entity)
        {
            if (entity == null) return null;

            return new CargoEstadiaDataModel
            {
                IdCargoEstadia = entity.IdCargoEstadia,
                CargoGuid = entity.CargoGuid,
                IdEstadia = entity.IdEstadia,
                IdCatalogo = entity.IdCatalogo,
                DescripcionCargo = entity.DescripcionCargo,
                Cantidad = entity.Cantidad,
                PrecioUnitario = entity.PrecioUnitario,
                Subtotal = entity.Subtotal,
                ValorIva = entity.ValorIva,
                TotalCargo = entity.TotalCargo,
                FechaConsumoUtc = entity.FechaConsumoUtc,
                EstadoCargo = entity.EstadoCargo,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion
            };
        }

        // Mapeo de CargoEstadiaDataModel a CargoEstadiaEntity
        public static CargoEstadiaEntity ToEntity(this CargoEstadiaDataModel model)
        {
            if (model == null) return null;

            return new CargoEstadiaEntity
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

        // Extensiones para listas de EstadiaEntity / EstadiaDataModel
        public static List<EstadiaDataModel> ToModelList(this IEnumerable<EstadiaEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<EstadiaEntity> ToEntityList(this IEnumerable<EstadiaDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();

        // Extensiones para listas de CargoEstadiaEntity / CargoEstadiaDataModel
        public static List<CargoEstadiaDataModel> ToModelList(this IEnumerable<CargoEstadiaEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<CargoEstadiaEntity> ToEntityList(this IEnumerable<CargoEstadiaDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}