using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Reservas;

namespace Servicio.Hotel.DataAccess.Configurations.Reservas
{
    public class ReservaHabitacionConfiguration : IEntityTypeConfiguration<ReservaHabitacionEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaHabitacionEntity> builder)
        {
            builder.ToTable("RESERVAS_HABITACIONES", "booking");
            builder.HasKey(e => e.IdReservaHabitacion);

            builder.Property(e => e.IdReservaHabitacion).HasColumnName("id_reserva_habitacion").ValueGeneratedOnAdd();
            builder.Property(e => e.ReservaHabitacionGuid).HasColumnName("reserva_habitacion_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdReserva).HasColumnName("id_reserva");
            builder.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            builder.Property(e => e.IdTarifa).HasColumnName("id_tarifa");
            builder.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            builder.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            builder.Property(e => e.NumAdultos).HasColumnName("num_adultos");
            builder.Property(e => e.NumNinos).HasColumnName("num_ninos");
            builder.Property(e => e.PrecioNocheAplicado).HasColumnName("precio_noche_aplicado").HasColumnType("decimal(12,2)");
            builder.Property(e => e.SubtotalLinea).HasColumnName("subtotal_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.ValorIvaLinea).HasColumnName("valor_iva_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.DescuentoLinea).HasColumnName("descuento_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalLinea).HasColumnName("total_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.EstadoDetalle).HasColumnName("estado_detalle").HasMaxLength(3);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("reservas-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.ReservaHabitacionGuid).IsUnique();
            builder.HasIndex(e => new { e.IdReserva, e.IdHabitacion, e.FechaInicio }).IsUnique();

            // Índice para validación de solapamiento (según script)
            builder.HasIndex(e => new { e.IdHabitacion, e.FechaInicio, e.FechaFin, e.EstadoDetalle })
                .HasDatabaseName("IX_RESERVAS_HABITACIONES_HAB_FECHAS");

            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.ReservasHabitaciones)
                .HasForeignKey(e => e.IdReserva);

            builder.HasCheckConstraint("CHK_RESERVAS_HABITACIONES_FECHAS", "[fecha_fin] > [fecha_inicio]");
            builder.HasCheckConstraint("CHK_RESERVAS_HABITACIONES_ADULTOS", "[num_adultos] > 0");
            builder.HasCheckConstraint("CHK_RESERVAS_HABITACIONES_ESTADO",
                "[estado_detalle] IN ('PEN','CON','CAN','FIN','EMI')");
        }
    }
}