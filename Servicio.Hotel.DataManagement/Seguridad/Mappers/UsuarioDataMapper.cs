using System.Collections.Generic;
using System;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Models;

namespace Servicio.Hotel.DataManagement.Seguridad.Mappers
{
    public static class UsuarioDataMapper
    {
        public static UsuarioDataModel? ToModel(this UsuarioAppEntity? entity)
        {
            if (entity == null) return null;

            return new UsuarioDataModel
            {
                IdUsuario = entity.IdUsuario,
                UsuarioGuid = entity.UsuarioGuid,
                IdCliente = entity.IdCliente,
                Username = entity.Username,
                Correo = entity.Correo,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                EstadoUsuario = entity.EstadoUsuario,
                EsEliminado = entity.EsEliminado,
                Activo = entity.Activo,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                RowVersion = entity.RowVersion,
                Roles = entity.UsuariosRoles?.Select(ur => ur.Rol?.ToModel()).Where(r => r != null).ToList()
            };
        }

        public static UsuarioAppEntity? ToEntity(this UsuarioDataModel? model)
        {
            if (model == null) return null;

            return new UsuarioAppEntity
            {
                IdUsuario = model.IdUsuario,
                UsuarioGuid = model.UsuarioGuid,
                IdCliente = model.IdCliente,
                Username = model.Username,
                Correo = model.Correo,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                PasswordHash = model.PasswordHash ?? string.Empty,
                PasswordSalt = model.PasswordSalt ?? string.Empty,
                EstadoUsuario = model.EstadoUsuario,
                EsEliminado = model.EsEliminado,
                Activo = model.Activo,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                RowVersion = model.RowVersion,
                UsuariosRoles = model.Roles?.Select(r => new UsuarioRolEntity
                {
                    IdRol = r.IdRol,
                    EstadoUsuarioRol = "ACT",
                    EsEliminado = false,
                    Activo = true,
                    FechaRegistroUtc = DateTime.UtcNow,
                    CreadoPorUsuario = string.IsNullOrWhiteSpace(model.CreadoPorUsuario) ? "Sistema" : model.CreadoPorUsuario
                }).ToList()
                // Nota: La relación UsuariosRoles se debe manejar aparte para evitar ciclos
            };
        }

        public static UsuarioCredencialesDataModel? ToCredentialsModel(this UsuarioAppEntity? entity)
        {
            if (entity == null) return null;

            return new UsuarioCredencialesDataModel
            {
                IdUsuario = entity.IdUsuario,
                Username = entity.Username,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt,
                EstadoUsuario = entity.EstadoUsuario,
                Activo = entity.Activo
            };
        }

        public static List<UsuarioDataModel> ToModelList(this IEnumerable<UsuarioAppEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<UsuarioAppEntity> ToEntityList(this IEnumerable<UsuarioDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}
