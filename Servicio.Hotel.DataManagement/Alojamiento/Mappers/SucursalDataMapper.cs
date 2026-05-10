using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class SucursalDataMapper
    {
        public static SucursalDataModel? ToModel(this SucursalEntity? entity)
        {
            if (entity == null) return null;

            return new SucursalDataModel
            {
                IdSucursal = entity.IdSucursal,
                SucursalGuid = entity.SucursalGuid,
                CodigoSucursal = entity.CodigoSucursal,
                NombreSucursal = entity.NombreSucursal,
                DescripcionSucursal = entity.DescripcionSucursal,
                DescripcionCorta = entity.DescripcionCorta,
                TipoAlojamiento = entity.TipoAlojamiento,
                Estrellas = entity.Estrellas,
                CategoriaViaje = entity.CategoriaViaje,
                Pais = entity.Pais,
                Provincia = entity.Provincia,
                Ciudad = entity.Ciudad,
                Ubicacion = entity.Ubicacion,
                Direccion = entity.Direccion,
                CodigoPostal = entity.CodigoPostal,
                Telefono = entity.Telefono,
                Correo = entity.Correo,
                Latitud = entity.Latitud,
                Longitud = entity.Longitud,
                HoraCheckin = entity.HoraCheckin,
                HoraCheckout = entity.HoraCheckout,
                CheckinAnticipado = entity.CheckinAnticipado,
                CheckoutTardio = entity.CheckoutTardio,
                AceptaNinos = entity.AceptaNinos,
                EdadMinimaHuesped = entity.EdadMinimaHuesped,
                PermiteMascotas = entity.PermiteMascotas,
                SePermiteFumar = entity.SePermiteFumar,
                EstadoSucursal = entity.EstadoSucursal,
                EsEliminado = entity.EsEliminado,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion,
                Imagenes = entity.Imagenes.ToSucursalImagenModelList()
            };
        }

        public static SucursalEntity? ToEntity(this SucursalDataModel? model)
        {
            if (model == null) return null;

            return new SucursalEntity
            {
                IdSucursal = model.IdSucursal,
                SucursalGuid = model.SucursalGuid,
                CodigoSucursal = model.CodigoSucursal,
                NombreSucursal = model.NombreSucursal,
                DescripcionSucursal = model.DescripcionSucursal,
                DescripcionCorta = model.DescripcionCorta,
                TipoAlojamiento = model.TipoAlojamiento,
                Estrellas = model.Estrellas,
                CategoriaViaje = model.CategoriaViaje,
                Pais = model.Pais,
                Provincia = model.Provincia,
                Ciudad = model.Ciudad,
                Ubicacion = model.Ubicacion,
                Direccion = model.Direccion,
                CodigoPostal = model.CodigoPostal,
                Telefono = model.Telefono,
                Correo = model.Correo,
                Latitud = model.Latitud,
                Longitud = model.Longitud,
                HoraCheckin = model.HoraCheckin,
                HoraCheckout = model.HoraCheckout,
                CheckinAnticipado = model.CheckinAnticipado,
                CheckoutTardio = model.CheckoutTardio,
                AceptaNinos = model.AceptaNinos,
                EdadMinimaHuesped = model.EdadMinimaHuesped,
                PermiteMascotas = model.PermiteMascotas,
                SePermiteFumar = model.SePermiteFumar,
                EstadoSucursal = model.EstadoSucursal,
                EsEliminado = model.EsEliminado,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static List<SucursalDataModel> ToModelList(this IEnumerable<SucursalEntity> entities)
            => entities?.Select(x => x.ToModel()!).ToList() ?? new();
    }
}
