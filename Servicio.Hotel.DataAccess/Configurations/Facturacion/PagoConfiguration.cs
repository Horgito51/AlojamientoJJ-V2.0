using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Configurations.Facturacion
{
    public class PagoConfiguration : IEntityTypeConfiguration<PagoEntity>
    {
        public void Configure(EntityTypeBuilder<PagoEntity> builder)
        {
            builder.ToTable("PAGOS", "booking");
            builder.HasKey(e => e.IdPago);

            builder.Property(e => e.IdPago).HasColumnName("id_pago").ValueGeneratedOnAdd();
            builder.Property(e => e.PagoGuid).HasColumnName("pago_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdFactura).HasColumnName("id_factura");
            builder.Property(e => e.IdReserva).HasColumnName("id_reserva");
            builder.Property(e => e.Monto).HasColumnName("monto").HasColumnType("decimal(12,2)");
            builder.Property(e => e.MetodoPago).HasColumnName("metodo_pago").HasMaxLength(40);
            builder.Property(e => e.EsPagoElectronico).HasColumnName("es_pago_electronico");
            builder.Property(e => e.ProveedorPasarela).HasColumnName("proveedor_pasarela").HasMaxLength(50);
            builder.Property(e => e.TransaccionExterna).HasColumnName("transaccion_externa").HasMaxLength(150);
            builder.Property(e => e.CodigoAutorizacion).HasColumnName("codigo_autorizacion").HasMaxLength(150);
            builder.Property(e => e.Referencia).HasColumnName("referencia").HasMaxLength(150);
            builder.Property(e => e.EstadoPago).HasColumnName("estado_pago").HasMaxLength(3);
            builder.Property(e => e.FechaPagoUtc).HasColumnName("fecha_pago_utc");
            builder.Property(e => e.Moneda).HasColumnName("moneda").HasMaxLength(10);
            builder.Property(e => e.TipoCambio).HasColumnName("tipo_cambio").HasColumnType("decimal(10,4)");
            builder.Property(e => e.RespuestaPasarela).HasColumnName("respuesta_pasarela");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.PagoGuid).IsUnique();
            builder.HasIndex(e => e.TransaccionExterna).IsUnique()
                .HasFilter("[transaccion_externa] IS NOT NULL");

            builder.HasOne(e => e.Factura)
                .WithMany(f => f.Pagos)
                .HasForeignKey(e => e.IdFactura);

            builder.HasCheckConstraint("CHK_PAGOS_MONTO", "[monto] > 0");
            builder.HasCheckConstraint("CHK_PAGOS_ESTADO",
                "[estado_pago] IN ('PEN','PRO','APR','REC','CAN')");
            builder.HasCheckConstraint("CHK_PAGOS_TIPO_CAMBIO", "[tipo_cambio] > 0");
        }
    }
}