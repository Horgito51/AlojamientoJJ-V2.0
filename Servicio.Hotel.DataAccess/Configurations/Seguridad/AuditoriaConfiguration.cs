using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Configurations.Seguridad
{
    public class AuditoriaConfiguration : IEntityTypeConfiguration<AuditoriaEntity>
    {
        public void Configure(EntityTypeBuilder<AuditoriaEntity> builder)
        {
            builder.ToTable("AUDITORIA", "seguridad");
            builder.HasKey(e => e.IdAuditoria);

            builder.Property(e => e.IdAuditoria).HasColumnName("id_auditoria").ValueGeneratedOnAdd();
            builder.Property(e => e.AuditoriaGuid).HasColumnName("auditoria_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.TablaAfectada).HasColumnName("tabla_afectada").HasMaxLength(100);
            builder.Property(e => e.Operacion).HasColumnName("operacion").HasMaxLength(10);
            builder.Property(e => e.IdRegistroAfectado).HasColumnName("id_registro_afectado").HasMaxLength(100);
            builder.Property(e => e.DatosAnteriores).HasColumnName("datos_anteriores");
            builder.Property(e => e.DatosNuevos).HasColumnName("datos_nuevos");
            builder.Property(e => e.UsuarioEjecutor).HasColumnName("usuario_ejecutor").HasMaxLength(100);
            builder.Property(e => e.IpOrigen).HasColumnName("ip_origen").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50);
            builder.Property(e => e.FechaEventoUtc).HasColumnName("fecha_evento_utc");
            builder.Property(e => e.Activo).HasColumnName("activo");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.AuditoriaGuid).IsUnique();
            builder.HasIndex(e => new { e.TablaAfectada, e.FechaEventoUtc })
                .HasDatabaseName("IX_AUDITORIA_TABLA_FECHA");

            builder.HasCheckConstraint("CHK_AUDITORIA_OPERACION", "[operacion] IN ('INSERT','UPDATE','DELETE')");
        }
    }
}