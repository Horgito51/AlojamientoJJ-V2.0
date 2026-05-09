using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;

namespace Servicio.Hotel.DataAccess.Configurations.Hospedaje
{
    public class CargoEstadiaConfiguration : IEntityTypeConfiguration<CargoEstadiaEntity>
    {
        public void Configure(EntityTypeBuilder<CargoEstadiaEntity> builder)
        {
            builder.ToTable("CARGO_ESTADIA", "booking");
            builder.HasKey(e => e.IdCargoEstadia);

            builder.Property(e => e.IdCargoEstadia).HasColumnName("id_cargo_estadia").ValueGeneratedOnAdd();
            builder.Property(e => e.CargoGuid).HasColumnName("cargo_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdEstadia).HasColumnName("id_estadia");
            builder.Property(e => e.IdCatalogo).HasColumnName("id_catalogo");
            builder.Property(e => e.DescripcionCargo).HasColumnName("descripcion_cargo").HasMaxLength(250);
            builder.Property(e => e.Cantidad).HasColumnName("cantidad");
            builder.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(12,2)");
            builder.Property(e => e.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)");
            builder.Property(e => e.ValorIva).HasColumnName("valor_iva").HasColumnType("decimal(12,2)");
            builder.Property(e => e.TotalCargo).HasColumnName("total_cargo").HasColumnType("decimal(12,2)");
            builder.Property(e => e.FechaConsumoUtc).HasColumnName("fecha_consumo_utc");
            builder.Property(e => e.EstadoCargo).HasColumnName("estado_cargo").HasMaxLength(3);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("hospedaje-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.CargoGuid).IsUnique();

            builder.HasOne(e => e.Estadia)
                .WithMany(e => e.CargosEstadia)
                .HasForeignKey(e => e.IdEstadia);

            builder.HasCheckConstraint("CHK_CARGO_ESTADIA_CANTIDAD", "[cantidad] > 0");
            builder.HasCheckConstraint("CHK_CARGO_ESTADIA_ESTADO", "[estado_cargo] IN ('PEN','FAC','ANU')");
        }
    }
}