using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class ImagenDataMapper
    {
        public static ImagenDataModel ToModel(this TipoHabitacionImagenEntity entity) => new()
        {
            ImagenGuid = CreateStableGuid("tipo-habitacion-imagen", entity.IdTipoHabitacionImagen),
            IdImagen = entity.IdTipoHabitacionImagen,
            UrlImagen = entity.UrlImagen,
            Descripcion = entity.DescripcionImagen,
            Orden = entity.OrdenVisualizacion,
            EsPrincipal = entity.EsPrincipal
        };

        public static ImagenDataModel ToModel(this SucursalImagenEntity entity, bool esPrincipal) => new()
        {
            ImagenGuid = entity.GuidSucursalImagen,
            IdImagen = entity.IdSucursalImagen,
            UrlImagen = entity.UrlImagen,
            Descripcion = entity.Descripcion,
            Orden = entity.Orden,
            EsPrincipal = esPrincipal
        };

        public static List<ImagenDataModel> ToTipoImagenModelList(this IEnumerable<TipoHabitacionImagenEntity>? entities)
            => entities?
                .OrderByDescending(i => i.EsPrincipal)
                .ThenBy(i => i.OrdenVisualizacion)
                .Select(i => i.ToModel())
                .ToList() ?? new();

        public static List<ImagenDataModel> ToSucursalImagenModelList(this IEnumerable<SucursalImagenEntity>? entities)
        {
            var active = entities?
                .Where(i => string.Equals(i.Estado, "ACT", StringComparison.OrdinalIgnoreCase))
                .OrderBy(i => i.Orden)
                .ThenBy(i => i.IdSucursalImagen)
                .ToList() ?? new();

            var principalId = active.FirstOrDefault()?.IdSucursalImagen;
            return active.Select(i => i.ToModel(principalId.HasValue && i.IdSucursalImagen == principalId.Value)).ToList();
        }

        private static Guid CreateStableGuid(string scope, int id)
        {
            var bytes = new byte[16];
            var scopeBytes = Encoding.UTF8.GetBytes(scope);
            Array.Copy(scopeBytes, bytes, Math.Min(12, scopeBytes.Length));
            BitConverter.GetBytes(id).CopyTo(bytes, 12);
            return new Guid(bytes);
        }
    }
}
