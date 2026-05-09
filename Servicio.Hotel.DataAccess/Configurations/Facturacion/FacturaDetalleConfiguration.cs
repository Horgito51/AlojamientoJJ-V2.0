using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Configurations.Facturacion
{
    public class FacturaDetalleConfiguration : IEntityTypeConfiguration<FacturaDetalleEntity>
    {
        public void Configure(EntityTypeBuilder<FacturaDetalleEntity> builder)
        {
            builder.ToTable("FACTURA_DETALLE", "booking");
            builder.HasKey(e => e.IdFacturaDetalle);

            builder.Property(e => e.IdFacturaDetalle).HasColumnName("id_factura_detalle").ValueGeneratedOnAdd();
            builder.Property(e => e.FacturaDetalleGuid).HasColumnName("factura_detalle_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdFactura).HasColumnName("id_factura");
            builder.Property(e => e.TipoItem).HasColumnName("tipo_item").HasMaxLength(30);
            builder.Property(e => e.ReferenciaTipo).HasColumnName("referencia_tipo").HasMaxLength(30);
            builder.Property(e => e.ReferenciaId).HasColumnName("referencia_id");
            builder.Property(e => e.DescripcionItem).HasColumnName("descripcion_item").HasMaxLength(250);
            builder.Property(e => e.Cantidad).HasColumnName("cantidad");
            builder.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(12,2)");
            builder.Property(e => e.SubtotalLinea).HasColumnName("subtotal_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.ValorIvaLinea).HasColumnName("valor_iva_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.DescuentoLinea).HasColumnName("descuento_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalLinea).HasColumnName("total_linea").HasColumnType("decimal(12,2)");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.FacturaDetalleGuid).IsUnique();

            builder.HasOne(e => e.Factura)
                .WithMany(f => f.FacturaDetalles)
                .HasForeignKey(e => e.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("CHK_FACTURA_DETALLE_TIPO",
                "[tipo_item] IN ('ALOJAMIENTO','SERVICIO','DESCUENTO','AJUSTE')");
            builder.HasCheckConstraint("CHK_FACTURA_DETALLE_CANTIDAD", "[cantidad] > 0");
        }
    }
}