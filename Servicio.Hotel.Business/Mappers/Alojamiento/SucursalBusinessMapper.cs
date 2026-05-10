using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class SucursalBusinessMapper
    {
        public static SucursalDTO? ToDto(this SucursalDataModel? model)
        {
            if (model == null) return null;

            return new SucursalDTO
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
                RowVersion = model.RowVersion,
                Imagenes = model.Imagenes.ToDtoList()
            };
        }

        public static SucursalDataModel? ToDataModel(this SucursalDTO? dto)
        {
            if (dto == null) return null;

            return new SucursalDataModel
            {
                IdSucursal = dto.IdSucursal,
                SucursalGuid = dto.SucursalGuid,
                CodigoSucursal = dto.CodigoSucursal,
                NombreSucursal = dto.NombreSucursal,
                DescripcionSucursal = dto.DescripcionSucursal,
                DescripcionCorta = dto.DescripcionCorta,
                TipoAlojamiento = dto.TipoAlojamiento,
                Estrellas = dto.Estrellas,
                CategoriaViaje = dto.CategoriaViaje,
                Pais = dto.Pais,
                Provincia = dto.Provincia,
                Ciudad = dto.Ciudad,
                Ubicacion = dto.Ubicacion,
                Direccion = dto.Direccion,
                CodigoPostal = dto.CodigoPostal,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                HoraCheckin = dto.HoraCheckin,
                HoraCheckout = dto.HoraCheckout,
                CheckinAnticipado = dto.CheckinAnticipado,
                CheckoutTardio = dto.CheckoutTardio,
                AceptaNinos = dto.AceptaNinos,
                EdadMinimaHuesped = dto.EdadMinimaHuesped,
                PermiteMascotas = dto.PermiteMascotas,
                SePermiteFumar = dto.SePermiteFumar,
                EstadoSucursal = dto.EstadoSucursal,
                EsEliminado = dto.EsEliminado,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }

        public static List<SucursalDTO> ToDtoList(this IEnumerable<SucursalDataModel> models)
            => models?.Select(x => x.ToDto()!).ToList() ?? new();

        public static SucursalDataModel ToDataModel(this SucursalCreateDTO dto)
        {
            if (dto == null) return null;

            return new SucursalDataModel
            {
                CodigoSucursal = dto.CodigoSucursal,
                NombreSucursal = dto.NombreSucursal,
                DescripcionSucursal = dto.DescripcionSucursal ?? string.Empty,
                DescripcionCorta = dto.DescripcionCorta ?? string.Empty,
                TipoAlojamiento = dto.TipoAlojamiento ?? string.Empty,
                Estrellas = dto.Estrellas,
                CategoriaViaje = dto.CategoriaViaje ?? string.Empty,
                Pais = dto.Pais ?? string.Empty,
                Provincia = dto.Provincia ?? string.Empty,
                Ciudad = dto.Ciudad ?? string.Empty,
                Ubicacion = dto.Ubicacion ?? string.Empty,
                Direccion = dto.Direccion ?? string.Empty,
                CodigoPostal = dto.CodigoPostal ?? string.Empty,
                Telefono = dto.Telefono ?? string.Empty,
                Correo = dto.Correo ?? string.Empty,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                HoraCheckin = dto.HoraCheckin ?? string.Empty,
                HoraCheckout = dto.HoraCheckout ?? string.Empty,
                CheckinAnticipado = dto.CheckinAnticipado,
                CheckoutTardio = dto.CheckoutTardio,
                AceptaNinos = dto.AceptaNinos,
                EdadMinimaHuesped = dto.EdadMinimaHuesped,
                PermiteMascotas = dto.PermiteMascotas,
                SePermiteFumar = dto.SePermiteFumar,
                EstadoSucursal = dto.EstadoSucursal,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }

        public static SucursalDataModel ToDataModel(this SucursalUpdateDTO dto)
        {
            if (dto == null) return null;

            return new SucursalDataModel
            {
                IdSucursal = dto.IdSucursal,
                CodigoSucursal = dto.CodigoSucursal,
                NombreSucursal = dto.NombreSucursal,
                DescripcionSucursal = dto.DescripcionSucursal ?? string.Empty,
                DescripcionCorta = dto.DescripcionCorta ?? string.Empty,
                TipoAlojamiento = dto.TipoAlojamiento ?? string.Empty,
                Estrellas = dto.Estrellas,
                CategoriaViaje = dto.CategoriaViaje ?? string.Empty,
                Pais = dto.Pais ?? string.Empty,
                Provincia = dto.Provincia ?? string.Empty,
                Ciudad = dto.Ciudad ?? string.Empty,
                Ubicacion = dto.Ubicacion ?? string.Empty,
                Direccion = dto.Direccion ?? string.Empty,
                CodigoPostal = dto.CodigoPostal ?? string.Empty,
                Telefono = dto.Telefono ?? string.Empty,
                Correo = dto.Correo ?? string.Empty,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                HoraCheckin = dto.HoraCheckin ?? string.Empty,
                HoraCheckout = dto.HoraCheckout ?? string.Empty,
                CheckinAnticipado = dto.CheckinAnticipado,
                CheckoutTardio = dto.CheckoutTardio,
                AceptaNinos = dto.AceptaNinos,
                EdadMinimaHuesped = dto.EdadMinimaHuesped,
                PermiteMascotas = dto.PermiteMascotas,
                SePermiteFumar = dto.SePermiteFumar,
                EstadoSucursal = dto.EstadoSucursal,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }
    }
}
