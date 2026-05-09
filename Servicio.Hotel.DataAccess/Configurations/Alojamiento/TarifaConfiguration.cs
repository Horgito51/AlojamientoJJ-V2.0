using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class TarifaConfiguration : IEntityTypeConfiguration<TarifaEntity>
    {
        public void Configure(EntityTypeBuilder<TarifaEntity> builder)
        {
            builder.ToTable("TARIFA", "booking");
            builder.HasKey(e => e.IdTarifa);

            builder.Property(e => e.IdTarifa).HasColumnName("id_tarifa").ValueGeneratedOnAdd();
            builder.Property(e => e.TarifaGuid).HasColumnName("tarifa_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.CodigoTarifa).HasColumnName("codigo_tarifa").HasMaxLength(30);
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            builder.Property(e => e.NombreTarifa).HasColumnName("nombre_tarifa").HasMaxLength(150);
            builder.Property(e => e.CanalTarifa).HasColumnName("canal_tarifa").HasMaxLength(30);
            builder.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            builder.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            builder.Property(e => e.PrecioPorNoche).HasColumnName("precio_por_noche").HasColumnType("decimal(12,2)");
            builder.Property(e => e.PorcentajeIva).HasColumnName("porcentaje_iva").HasColumnType("decimal(5,2)");
            builder.Property(e => e.MinNoches).HasColumnName("min_noches");
            builder.Property(e => e.MaxNoches).HasColumnName("max_noches");
            builder.Property(e => e.PermitePortalPublico).HasColumnName("permite_portal_publico");
            builder.Property(e => e.Prioridad).HasColumnName("prioridad");
            builder.Property(e => e.EstadoTarifa).HasColumnName("estado_tarifa").HasMaxLength(3);
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("tarifas-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.TarifaGuid).IsUnique();
            builder.HasIndex(e => e.CodigoTarifa).IsUnique();

            builder.HasOne(e => e.Sucursal)
                .WithMany(s => s.Tarifas)
                .HasForeignKey(e => e.IdSucursal);

            builder.HasOne(e => e.TipoHabitacion)
                .WithMany(th => th.Tarifas)
                .HasForeignKey(e => e.IdTipoHabitacion);

            builder.HasCheckConstraint("CHK_TARIFA_ESTADO", "[estado_tarifa] IN ('ACT','INA')");
            builder.HasCheckConstraint("CHK_TARIFA_FECHAS", "[fecha_fin] >= [fecha_inicio]");
            builder.HasCheckConstraint("CHK_TARIFA_PRECIO", "[precio_por_noche] > 0");
        }
    }
}