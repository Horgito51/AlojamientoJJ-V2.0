using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;

namespace Servicio.Hotel.DataAccess.Configurations.Valoraciones
{
    public class ValoracionConfiguration : IEntityTypeConfiguration<ValoracionEntity>
    {
        public void Configure(EntityTypeBuilder<ValoracionEntity> builder)
        {
            builder.ToTable("VALORACIONES", "booking");
            builder.HasKey(e => e.IdValoracion);

            builder.Property(e => e.IdValoracion).HasColumnName("id_valoracion").ValueGeneratedOnAdd();
            builder.Property(e => e.ValoracionGuid).HasColumnName("valoracion_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdEstadia).HasColumnName("id_estadia");
            builder.Property(e => e.IdCliente).HasColumnName("id_cliente");
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            builder.Property(e => e.PuntuacionGeneral).HasColumnName("puntuacion_general").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionLimpieza).HasColumnName("puntuacion_limpieza").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionConfort).HasColumnName("puntuacion_confort").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionUbicacion).HasColumnName("puntuacion_ubicacion").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionInstalaciones).HasColumnName("puntuacion_instalaciones").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionPersonal).HasColumnName("puntuacion_personal").HasColumnType("decimal(3,1)");
            builder.Property(e => e.PuntuacionCalidadPrecio).HasColumnName("puntuacion_calidad_precio").HasColumnType("decimal(3,1)");
            builder.Property(e => e.ComentarioPositivo).HasColumnName("comentario_positivo");
            builder.Property(e => e.ComentarioNegativo).HasColumnName("comentario_negativo");
            builder.Property(e => e.TipoViaje).HasColumnName("tipo_viaje").HasMaxLength(20);
            builder.Property(e => e.EstadoValoracion).HasColumnName("estado_valoracion").HasMaxLength(3);
            builder.Property(e => e.PublicadaEnPortal).HasColumnName("publicada_en_portal");
            builder.Property(e => e.RespuestaHotel).HasColumnName("respuesta_hotel");
            builder.Property(e => e.FechaRespuestaUtc).HasColumnName("fecha_respuesta_utc");
            builder.Property(e => e.ModeradaPorUsuario).HasColumnName("moderada_por_usuario").HasMaxLength(100);
            builder.Property(e => e.MotivoModeracion).HasColumnName("motivo_moderacion").HasMaxLength(250);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("reputacion-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.ValoracionGuid).IsUnique();
            builder.HasIndex(e => new { e.IdEstadia, e.IdCliente }).IsUnique();
            builder.HasIndex(e => new { e.IdSucursal, e.EstadoValoracion, e.PublicadaEnPortal })
                .HasDatabaseName("IX_VALORACIONES_SUCURSAL_ESTADO");

            builder.HasCheckConstraint("CHK_VALORACIONES_PUNTUACION", "[puntuacion_general] BETWEEN 0 AND 10");
            builder.HasCheckConstraint("CHK_VALORACIONES_TIPO_VIAJE",
                "[tipo_viaje] IS NULL OR [tipo_viaje] IN ('pareja','familia','negocios','amigos','solo')");
            builder.HasCheckConstraint("CHK_VALORACIONES_ESTADO",
                "[estado_valoracion] IN ('PEN','PUB','OCU','REP')");
        }
    }
}