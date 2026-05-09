using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;

namespace Servicio.Hotel.DataAccess.Configurations.Hospedaje
{
    public class EstadiaConfiguration : IEntityTypeConfiguration<EstadiaEntity>
    {
        public void Configure(EntityTypeBuilder<EstadiaEntity> builder)
        {
            builder.ToTable("ESTADIAS", "booking");
            builder.HasKey(e => e.IdEstadia);

            builder.Property(e => e.IdEstadia).HasColumnName("id_estadia").ValueGeneratedOnAdd();
            builder.Property(e => e.EstadiaGuid).HasColumnName("estadia_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdReservaHabitacion).HasColumnName("id_reserva_habitacion");
            builder.Property(e => e.IdCliente).HasColumnName("id_cliente");
            builder.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            builder.Property(e => e.CheckinUtc).HasColumnName("checkin_utc");
            builder.Property(e => e.CheckoutUtc).HasColumnName("checkout_utc");
            builder.Property(e => e.EstadoEstadia).HasColumnName("estado_estadia").HasMaxLength(3);
            builder.Property(e => e.ObservacionesCheckin).HasColumnName("observaciones_checkin");
            builder.Property(e => e.ObservacionesCheckout).HasColumnName("observaciones_checkout");
            builder.Property(e => e.RequiereMantenimiento).HasColumnName("requiere_mantenimiento");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("hospedaje-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.EstadiaGuid).IsUnique();
            builder.HasIndex(e => e.IdReservaHabitacion).IsUnique(); // UQ_ESTADIAS_RESERVA_HAB

            builder.HasCheckConstraint("CHK_ESTADIAS_ESTADO", "[estado_estadia] IN ('ACT','FIN','CAN')");
            builder.HasCheckConstraint("CHK_ESTADIAS_FECHAS",
                "[checkout_utc] IS NULL OR [checkin_utc] IS NULL OR [checkout_utc] >= [checkin_utc]");
        }
    }
}