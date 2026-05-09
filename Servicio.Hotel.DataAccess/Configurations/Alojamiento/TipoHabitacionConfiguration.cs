using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class TipoHabitacionConfiguration : IEntityTypeConfiguration<TipoHabitacionEntity>
    {
        public void Configure(EntityTypeBuilder<TipoHabitacionEntity> builder)
        {
            builder.ToTable("TIPO_HABITACION", "booking");
            builder.HasKey(e => e.IdTipoHabitacion);

            builder.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion").ValueGeneratedOnAdd();
            builder.Property(e => e.TipoHabitacionGuid).HasColumnName("tipo_habitacion_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.CodigoTipoHabitacion).HasColumnName("codigo_tipo_habitacion").HasMaxLength(30);
            builder.Property(e => e.NombreTipoHabitacion).HasColumnName("nombre_tipo_habitacion").HasMaxLength(120);
            builder.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(160);
            builder.Property(e => e.Descripcion).HasColumnName("descripcion");
            builder.Property(e => e.CapacidadAdultos).HasColumnName("capacidad_adultos");
            builder.Property(e => e.CapacidadNinos).HasColumnName("capacidad_ninos");
            builder.Property(e => e.CapacidadTotal).HasColumnName("capacidad_total");
            builder.Property(e => e.TipoCama).HasColumnName("tipo_cama").HasMaxLength(60);
            builder.Property(e => e.AreaM2).HasColumnName("area_m2").HasColumnType("decimal(6,2)");
            builder.Property(e => e.PermiteEventos).HasColumnName("permite_eventos");
            builder.Property(e => e.PermiteReservaPublica).HasColumnName("permite_reserva_publica");
            builder.Property(e => e.EstadoTipoHabitacion).HasColumnName("estado_tipo_habitacion").HasMaxLength(3);
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("habitaciones-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.TipoHabitacionGuid).IsUnique();
            builder.HasIndex(e => e.CodigoTipoHabitacion).IsUnique();
            builder.HasIndex(e => e.NombreTipoHabitacion).IsUnique();
            builder.HasIndex(e => e.Slug).IsUnique();

            builder.HasCheckConstraint("CHK_TIPO_HABITACION_ESTADO", "[estado_tipo_habitacion] IN ('ACT','INA')");
            builder.HasCheckConstraint("CHK_TIPO_HABITACION_ADULTOS", "[capacidad_adultos] > 0");
            builder.HasCheckConstraint("CHK_TIPO_HABITACION_TOTAL", "[capacidad_total] > 0");
        }
    }
}
