using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class TipoHabitacionImagenConfiguration : IEntityTypeConfiguration<TipoHabitacionImagenEntity>
    {
        public void Configure(EntityTypeBuilder<TipoHabitacionImagenEntity> builder)
        {
            builder.ToTable("TIPO_HABITACION_IMAGEN", "booking");
            builder.HasKey(e => e.IdTipoHabitacionImagen);

            builder.Property(e => e.IdTipoHabitacionImagen).HasColumnName("id_tipo_habitacion_imagen").ValueGeneratedOnAdd();
            builder.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            builder.Property(e => e.UrlImagen).HasColumnName("url_imagen").HasMaxLength(500);
            builder.Property(e => e.DescripcionImagen).HasColumnName("descripcion_imagen").HasMaxLength(255);
            builder.Property(e => e.OrdenVisualizacion).HasColumnName("orden_visualizacion");
            builder.Property(e => e.EsPrincipal).HasColumnName("es_principal");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasOne(e => e.TipoHabitacion)
                .WithMany(th => th.TipoHabitacionImagenes)
                .HasForeignKey(e => e.IdTipoHabitacion)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("CHK_TIPO_HABITACION_IMAGEN_ORDEN", "[orden_visualizacion] > 0");
        }
    }
}