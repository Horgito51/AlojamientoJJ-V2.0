using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class SucursalImagenConfiguration : IEntityTypeConfiguration<SucursalImagenEntity>
    {
        public void Configure(EntityTypeBuilder<SucursalImagenEntity> builder)
        {
            builder.ToTable("SUCURSAL_IMAGENES", "booking");
            builder.HasKey(e => e.IdSucursalImagen);

            builder.Property(e => e.IdSucursalImagen).HasColumnName("id_sucursal_imagen").ValueGeneratedOnAdd();
            builder.Property(e => e.GuidSucursalImagen).HasColumnName("guid_sucursal_imagen").ValueGeneratedOnAdd();
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.UrlImagen).HasColumnName("url_imagen").HasMaxLength(1000);
            builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(300);
            builder.Property(e => e.Orden).HasColumnName("orden");
            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(20);
            builder.Property(e => e.FechaCreacionUtc).HasColumnName("fecha_creacion_utc");
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");

            builder.HasOne(e => e.Sucursal)
                .WithMany(s => s.Imagenes)
                .HasForeignKey(e => e.IdSucursal)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.IdSucursal).HasDatabaseName("IX_SUCURSAL_IMAGENES_SUCURSAL");
        }
    }
}
