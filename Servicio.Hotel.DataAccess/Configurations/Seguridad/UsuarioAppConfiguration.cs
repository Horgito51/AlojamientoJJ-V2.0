using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Configurations.Seguridad
{
    public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
        {
            builder.ToTable("USUARIO_APP", "seguridad");
            builder.HasKey(e => e.IdUsuario);

            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario").ValueGeneratedOnAdd();
            builder.Property(e => e.UsuarioGuid).HasColumnName("usuario_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdCliente).HasColumnName("id_cliente");
            builder.Property(e => e.Username).HasColumnName("username").HasMaxLength(50);
            builder.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(120);
            builder.Property(e => e.Nombres).HasColumnName("nombres").HasMaxLength(120);
            builder.Property(e => e.Apellidos).HasColumnName("apellidos").HasMaxLength(120);
            builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(500);
            builder.Property(e => e.PasswordSalt).HasColumnName("password_salt").HasMaxLength(250);
            builder.Property(e => e.EstadoUsuario).HasColumnName("estado_usuario").HasMaxLength(3);
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

            builder.HasIndex(e => e.UsuarioGuid).IsUnique();
            builder.HasIndex(e => e.Username).IsUnique();
            builder.HasIndex(e => e.Correo).IsUnique();
            builder.HasIndex(e => new { e.EstadoUsuario, e.Activo, e.Correo })
                .HasDatabaseName("IX_USUARIO_APP_ESTADO");
            builder.HasIndex(e => e.IdCliente)
                .HasDatabaseName("IX_USUARIO_APP_CLIENTE")
                .HasFilter("[id_cliente] IS NOT NULL");

            builder.HasCheckConstraint("CHK_USUARIO_APP_ESTADO", "[estado_usuario] IN ('ACT','INA','BLO')");
        }
    }
}