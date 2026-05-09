using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Hospedaje
{
    public class EstadiaEntity
    {
        public int IdEstadia { get; set; }
        public Guid EstadiaGuid { get; set; }
        public int IdReservaHabitacion { get; set; }
        public int IdCliente { get; set; }
        public int IdHabitacion { get; set; }
        public DateTime? CheckinUtc { get; set; }
        public DateTime? CheckoutUtc { get; set; }
        public string EstadoEstadia { get; set; }
        public string? ObservacionesCheckin { get; set; }
        public string? ObservacionesCheckout { get; set; }
        public bool RequiereMantenimiento { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<CargoEstadiaEntity> CargosEstadia { get; set; }
    }
}
