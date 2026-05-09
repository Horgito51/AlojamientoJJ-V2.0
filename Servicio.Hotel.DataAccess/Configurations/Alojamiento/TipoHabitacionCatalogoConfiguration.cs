using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class TipoHabitacionCatalogoConfiguration : IEntityTypeConfiguration<TipoHabitacionCatalogoEntity>
    {
        public void Configure(EntityTypeBuilder<TipoHabitacionCatalogoEntity> builder)
        {
            builder.ToTable("TIPO_HABITACION_CATALOGO", "booking");
            builder.HasKey(e => e.IdTipoHabCatalogo);

            builder.Property(e => e.IdTipoHabCatalogo).HasColumnName("id_tipo_hab_catalogo").ValueGeneratedOnAdd();
            builder.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            builder.Property(e => e.IdCatalogo).HasColumnName("id_catalogo");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => new { e.IdTipoHabitacion, e.IdCatalogo }).IsUnique();

            builder.HasOne(e => e.TipoHabitacion)
                .WithMany(th => th.TipoHabitacionCatalogos)
                .HasForeignKey(e => e.IdTipoHabitacion)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.CatalogoServicio)
                .WithMany(cs => cs.TipoHabitacionCatalogos)
                .HasForeignKey(e => e.IdCatalogo);
        }
    }
}