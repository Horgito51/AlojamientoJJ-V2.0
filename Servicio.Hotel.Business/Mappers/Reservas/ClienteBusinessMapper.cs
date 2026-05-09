using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.DataManagement.Reservas.Models;

namespace Servicio.Hotel.Business.Mappers.Reservas
{
    public static class ClienteBusinessMapper
    {
        public static ClienteDTO ToDto(this ClienteDataModel model)
        {
            if (model == null) return null;

            return new ClienteDTO
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

        public static ClienteDataModel ToDataModel(this ClienteDTO dto)
        {
            if (dto == null) return null;

            return new ClienteDataModel
            {
                IdCliente = dto.IdCliente,
                ClienteGuid = dto.ClienteGuid,
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                RazonSocial = dto.RazonSocial,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Estado = dto.Estado,
                EsEliminado = dto.EsEliminado,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                RowVersion = dto.RowVersion
            };
        }

        public static List<ClienteDTO> ToDtoList(this IEnumerable<ClienteDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<ClienteDataModel> ToDataModelList(this IEnumerable<ClienteDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static ClienteDataModel ToDataModel(this ClienteCreateDTO dto)
        {
            if (dto == null) return null;

            return new ClienteDataModel
            {
                TipoIdentificacion = dto.TipoIdentificacion,
                NumeroIdentificacion = dto.NumeroIdentificacion,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                RazonSocial = dto.RazonSocial ?? string.Empty,
                Correo = dto.Correo,
                Telefono = dto.Telefono ?? string.Empty,
                Direccion = dto.Direccion ?? string.Empty,
                Estado = dto.Estado
            };
        }

        public static ClienteDataModel ToDataModel(this ClienteUpdateDTO dto)
        {
            if (dto == null) return null;

            return new ClienteDataModel
            {
                IdCliente = dto.IdCliente,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                RazonSocial = dto.RazonSocial ?? string.Empty,
                Correo = dto.Correo,
                Telefono = dto.Telefono ?? string.Empty,
                Direccion = dto.Direccion ?? string.Empty,
                Estado = dto.Estado
            };
        }
    }
}
