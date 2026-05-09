using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class HabitacionConfiguration : IEntityTypeConfiguration<HabitacionEntity>
    {
        public void Configure(EntityTypeBuilder<HabitacionEntity> builder)
        {
            builder.ToTable("HABITACION", "booking");
            builder.HasKey(e => e.IdHabitacion);

            builder.Property(e => e.IdHabitacion).HasColumnName("id_habitacion").ValueGeneratedOnAdd();
            builder.Property(e => e.HabitacionGuid).HasColumnName("habitacion_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            builder.Property(e => e.NumeroHabitacion).HasColumnName("numero_habitacion").HasMaxLength(20);
            builder.Property(e => e.Piso).HasColumnName("piso");
            builder.Property(e => e.CapacidadHabitacion).HasColumnName("capacidad_habitacion");
            builder.Property(e => e.PrecioBase).HasColumnName("precio_base").HasColumnType("decimal(12,2)");
            builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(500);
            builder.Property(e => e.DescripcionHabitacion).HasColumnName("descripcion_habitacion").HasMaxLength(250);
            builder.Property(e => e.EstadoHabitacion).HasColumnName("estado_habitacion").HasMaxLength(3);
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

            builder.HasIndex(e => e.HabitacionGuid).IsUnique();
            builder.HasIndex(e => new { e.IdSucursal, e.NumeroHabitacion }).IsUnique();

            builder.HasOne(e => e.Sucursal)
                .WithMany(s => s.Habitaciones)
                .HasForeignKey(e => e.IdSucursal);

            builder.HasOne(e => e.TipoHabitacion)
                .WithMany(th => th.Habitaciones)
                .HasForeignKey(e => e.IdTipoHabitacion);

            builder.HasCheckConstraint("CHK_HABITACION_ESTADO", "[estado_habitacion] IN ('DIS','OCU','MNT','FDS','INA')");
            builder.HasCheckConstraint("CHK_HABITACION_PRECIO", "[precio_base] > 0");
        }
    }
}
