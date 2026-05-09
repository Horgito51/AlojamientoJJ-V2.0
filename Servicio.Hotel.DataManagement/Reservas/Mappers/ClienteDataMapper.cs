using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataManagement.Reservas.Models;

namespace Servicio.Hotel.DataManagement.Reservas.Mappers
{
    public static class ClienteDataMapper
    {
        public static ClienteDataModel ToModel(this ClienteEntity entity)
        {
            if (entity == null) return null;

            return new ClienteDataModel
            {
                IdCliente = entity.IdCliente,
                ClienteGuid = entity.ClienteGuid,
                TipoIdentificacion = entity.TipoIdentificacion,
                NumeroIdentificacion = entity.NumeroIdentificacion,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                RazonSocial = entity.RazonSocial,
                Correo = entity.Correo,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,
                Estado = entity.Estado,
                EsEliminado = entity.EsEliminado,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                RowVersion = entity.RowVersion
            };
        }

        public static ClienteEntity ToEntity(this ClienteDataModel model)
        {
            if (model == null) return null;

            return new ClienteEntity
            {
                IdCliente = model.IdCliente,
                ClienteGuid = model.ClienteGuid,
                TipoIdentificacion = model.TipoIdentificacion,
                NumeroIdentificacion = model.NumeroIdentificacion,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                RazonSocial = model.RazonSocial,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                Estado = model.Estado,
                EsEliminado = model.EsEliminado,
                CreadoPorUsuario = model.CreadoPorUsuario,
                FechaRegistroUtc = model.FechaRegistroUtc,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                RowVersion = model.RowVersion
            };
        }

        public static List<ClienteDataModel> ToModelList(this IEnumerable<ClienteEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<ClienteEntity> ToEntityList(this IEnumerable<ClienteDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}