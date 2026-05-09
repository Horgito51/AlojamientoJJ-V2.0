using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Configurations.Facturacion
{
    public class FacturaConfiguration : IEntityTypeConfiguration<FacturaEntity>
    {
        public void Configure(EntityTypeBuilder<FacturaEntity> builder)
        {
            builder.ToTable("FACTURAS", "booking");
            builder.HasKey(e => e.IdFactura);

            builder.Property(e => e.IdFactura).HasColumnName("id_factura").ValueGeneratedOnAdd();
            builder.Property(e => e.GuidFactura).HasColumnName("guid_factura").ValueGeneratedOnAdd();
            builder.Property(e => e.IdCliente).HasColumnName("id_cliente");
            builder.Property(e => e.IdReserva).HasColumnName("id_reserva");
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.NumeroFactura).HasColumnName("numero_factura").HasMaxLength(40);
            builder.Property(e => e.TipoFactura).HasColumnName("tipo_factura").HasMaxLength(20);
            builder.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            builder.Property(e => e.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)");
            builder.Property(e => e.ValorIva).HasColumnName("valor_iva").HasColumnType("decimal(12,2)");
            builder.Property(e => e.DescuentoTotal).HasColumnName("descuento_total").HasColumnType("decimal(12,2)");
            builder.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal(12,2)");
            builder.Property(e => e.SaldoPendiente).HasColumnName("saldo_pendiente").HasColumnType("decimal(12,2)");
            builder.Property(e => e.Moneda).HasColumnName("moneda").HasMaxLength(10);
            builder.Property(e => e.ObservacionesFactura).HasColumnName("observaciones_factura").HasMaxLength(300);
            builder.Property(e => e.OrigenCanalFactura).HasColumnName("origen_canal_factura").HasMaxLength(50);
            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(3);
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("facturacion-service");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.GuidFactura).IsUnique();
            builder.HasIndex(e => e.NumeroFactura).IsUnique();

            builder.HasCheckConstraint("CHK_FACTURAS_TIPO", "[tipo_factura] IN ('RESERVA','FINAL','AJUSTE')");
            builder.HasCheckConstraint("CHK_FACTURAS_ESTADO", "[estado] IN ('EMI','PAG','ANU')");
            builder.HasCheckConstraint("CHK_FACTURAS_TOTAL_COHERENTE",
                "[total] >= [subtotal] - [descuento_total]");
        }
    }
}