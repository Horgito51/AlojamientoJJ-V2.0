using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Booking
{
    public class SearchResponseDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public PaginationMetadataDTO Metadata { get; set; }
        public List<AccommodationDTO> Data { get; set; }
        public List<OfertaDTO> OfertasDestacadas { get; set; }
        public List<DestinoDTO> DestinosPopulares { get; set; }
    }

    public class PaginationMetadataDTO
    {
        public int TotalResultados { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int Limite { get; set; }
        public bool TieneSiguiente { get; set; }
        public bool TieneAnterior { get; set; }
    }

    public class OfertaDTO
    {
        public bool TieneOferta { get; set; }
        public string TipoOferta { get; set; }
        public decimal? DescuentoPorcentaje { get; set; }
        public decimal? PrecioAntes { get; set; }
        public decimal PrecioOferta { get; set; }
        public DateTime FechaInicioOferta { get; set; }
        public DateTime FechaFinOferta { get; set; }
    }

    public class DestinoDTO
    {
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Categoria { get; set; }
    }
}