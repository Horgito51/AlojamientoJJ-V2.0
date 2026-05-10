using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class ImagenBusinessMapper
    {
        public static ImagenDTO ToDto(this ImagenDataModel model) => new()
        {
            ImagenGuid = model.ImagenGuid,
            IdImagen = model.IdImagen,
            UrlImagen = model.UrlImagen,
            Descripcion = model.Descripcion,
            Orden = model.Orden,
            EsPrincipal = model.EsPrincipal
        };

        public static ImagenDataModel ToDataModel(this ImagenDTO dto) => new()
        {
            ImagenGuid = dto.ImagenGuid,
            IdImagen = dto.IdImagen,
            UrlImagen = dto.UrlImagen,
            Descripcion = dto.Descripcion,
            Orden = dto.Orden,
            EsPrincipal = dto.EsPrincipal
        };

        public static List<ImagenDTO> ToDtoList(this IEnumerable<ImagenDataModel>? models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<ImagenDataModel> ToDataModelList(this IEnumerable<ImagenDTO>? dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();
    }
}
