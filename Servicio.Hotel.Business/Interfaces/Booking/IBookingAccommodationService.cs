using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.DTOs.Booking;

namespace Servicio.Hotel.Business.Interfaces.Booking
{
    public interface IBookingAccommodationService
    {
        Task<BookingPagedResponseDTO<AccommodationSearchItemDTO>> SearchAsync(AccommodationSearchQueryDTO query, CancellationToken ct = default);
        Task<AccommodationDetailResponseDTO> GetDetailAsync(Guid sucursalGuid, DateTime? fechaEntrada, DateTime? fechaSalida, CancellationToken ct = default);
        Task<BookingPagedResponseDTO<AccommodationReviewDTO>> GetReviewsAsync(Guid sucursalGuid, int pagina, int limite, CancellationToken ct = default);
        Task<IEnumerable<HabitacionPublicListItemDTO>> GetHabitacionesDisponiblesAsync(Guid sucursalGuid, DateTime fechaEntrada, DateTime fechaSalida, CancellationToken ct = default);
    }
}
