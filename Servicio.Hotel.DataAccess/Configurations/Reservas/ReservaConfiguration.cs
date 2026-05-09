using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Reservas;

namespace Servicio.Hotel.DataAccess.Configurations.Reservas
{
    public class ReservaConfiguration : IEntityTypeConfiguration<ReservaEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaEntity> builder)
        {
            builder.ToTable("RESERVAS", "booking");
            builder.HasKey(e => e.IdReserva);

            builder.Property(e => e.IdReserva).HasColumnName("id_reserva").ValueGeneratedOnAdd();
            builder.Property(e => e.GuidReserva).HasColumnName("guid_reserva").ValueGeneratedOnAdd();
            builder.Property(e => e.CodigoReserva).HasColumnName("codigo_reserva").HasMaxLength(40);
            builder.Property(e => e.IdCliente).HasColumnName("id_cliente");
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.FechaReservaUtc).HasColumnName("fecha_reserva_utc");
            builder.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            builder.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            builder.Property(e => e.SubtotalReserva).HasColumnName("subtotal_reserva").HasColumnType("decimal(12,2)");
            builder.Property(e => e.ValorIva).HasColumnName("valor_iva").HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalReserva).HasColumnName("total_reserva").HasColumnType("decimal(12,2)");
            builder.Property(e => e.DescuentoAplicado).HasColumnName("descuento_aplicado").HasColumnType("decimal(12,2)");
            builder.Property(e => e.SaldoPendiente).HasColumnName("saldo_pendiente").HasColumnType("decimal(12,2)");
            builder.Property(e => e.OrigenCanalReserva).HasColumnName("origen_canal_reserva").HasMaxLength(50);
            builder.Property(e => e.EstadoReserva).HasColumnName("estado_reserva").HasMaxLength(3);
            builder.Property(e => e.FechaConfirmacionUtc).HasColumnName("fecha_confirmacion_utc");
            builder.Property(e => e.FechaCancelacionUtc).HasColumnName("fecha_cancelacion_utc");
            builder.Property(e => e.MotivoCancelacion).HasColumnName("motivo_cancelacion").HasMaxLength(250);
            builder.Property(e => e.Observaciones).HasColumnName("observaciones");
            builder.Property(e => e.EsWalkin).HasColumnName("es_walkin");
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("reservas-service");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.GuidReserva).IsUnique();
            builder.HasIndex(e => e.CodigoReserva).IsUnique();

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(e => e.IdCliente);

            // Sucursal navigation (to be added when SucursalEntity is available)
            // builder.HasOne(e => e.Sucursal)
            //     .WithMany()
            //     .HasForeignKey(e => e.IdSucursal);

            builder.HasCheckConstraint("CHK_RESERVAS_ESTADO",
                "[estado_reserva] IN ('PEN','CON','CAN','EXP','FIN','EMI')");
            builder.HasCheckConstraint("CHK_RESERVAS_FECHAS", "[fecha_fin] > [fecha_inicio]");
            builder.HasCheckConstraint("CHK_RESERVAS_TOTAL_COHERENTE",
                "[total_reserva] >= [subtotal_reserva] - [descuento_aplicado]");
        }
    }
}