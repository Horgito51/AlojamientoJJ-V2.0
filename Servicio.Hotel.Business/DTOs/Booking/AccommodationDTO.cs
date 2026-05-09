using Servicio.Hotel.Business.DTOs.Alojamiento;
using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Booking
{
    public class AccommodationDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string TipoAlojamiento { get; set; }
        public int? Estrellas { get; set; }
        public string CategoriaViaje { get; set; }
        public string DescripcionCorta { get; set; }
        public string ImagenPrincipal { get; set; }
        public List<string> GaleriaImagenes { get; set; }
        public UbicacionDTO Ubicacion { get; set; }
        public RatingDTO Rating { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PrecioTotalConImpuestos { get; set; }
        public string Moneda { get; set; }
        public object Oferta { get; set; } // Se puede tipificar después
        public List<TipoHabitacionDTO> HabitacionesDisponibles { get; set; }
        public List<ServicioDTO> Servicios { get; set; }
        public PoliticasDTO Politicas { get; set; }
        public DisponibilidadDTO Disponibilidad { get; set; }
        public LinksDTO Links { get; set; }
    }

    public class UbicacionDTO
    {
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Barrio { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string DistanciaCentro { get; set; }
        public string DistanciaAeropuerto { get; set; }
        public string UrlMapa { get; set; }
    }

    public class RatingDTO
    {
        public decimal PuntuacionGeneral { get; set; }
        public string ClasificacionTexto { get; set; }
        public int TotalResenas { get; set; }
        public decimal? PuntuacionLimpieza { get; set; }
        public decimal? PuntuacionConfort { get; set; }
        public decimal? PuntuacionUbicacion { get; set; }
        public decimal? PuntuacionInstalaciones { get; set; }
        public decimal? PuntuacionPersonal { get; set; }
        public decimal? PuntuacionRelacionCalidadPrecio { get; set; }
        public decimal PorcentajeRecomendacion { get; set; }
    }

    public class ServicioDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CategoriaServicio { get; set; }
        public bool EsGratuito { get; set; }
        public decimal? CostoAdicional { get; set; }
        public string IconoUrl { get; set; }
    }

    public class PoliticasDTO
    {
        public string HoraCheckin { get; set; }
        public string HoraCheckout { get; set; }
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
        public string PoliticaCancelacionGeneral { get; set; }
        public bool AceptaNinos { get; set; }
        public int? EdadMinimaHuesped { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
    }

    public class DisponibilidadDTO
    {
        public bool Disponible { get; set; }
        public int? UnidadesRestantes { get; set; }
        public string AlertaDisponibilidad { get; set; }
        public DateTime FechaConsulta { get; set; }
    }

    public class LinksDTO
    {
        public string Self { get; set; }
        public string Detalle { get; set; }
        public string Reservar { get; set; }
        public string Resenas { get; set; }
        public string Mapa { get; set; }
        public string Habitaciones { get; set; }
        public string Similares { get; set; }
    }
}