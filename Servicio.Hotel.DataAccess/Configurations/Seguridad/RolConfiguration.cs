using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Configurations.Seguridad
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEntity>
    {
        public void Configure(EntityTypeBuilder<RolEntity> builder)
        {
            builder.ToTable("ROL", "seguridad");
            builder.HasKey(e => e.IdRol);

            builder.Property(e => e.IdRol).HasColumnName("id_rol").ValueGeneratedOnAdd();
            builder.Property(e => e.RolGuid).HasColumnName("rol_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.NombreRol).HasColumnName("nombre_rol").HasMaxLength(50);
            builder.Property(e => e.DescripcionRol).HasColumnName("descripcion_rol").HasMaxLength(250);
            builder.Property(e => e.EstadoRol).HasColumnName("estado_rol").HasMaxLength(3);
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.Activo).HasColumnName("activo");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.RolGuid).IsUnique();
            builder.HasIndex(e => e.NombreRol).IsUnique();

            builder.HasCheckConstraint("CHK_ROL_ESTADO", "[estado_rol] IN ('ACT','INA')");
        }
    }
}