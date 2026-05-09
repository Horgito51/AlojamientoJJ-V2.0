using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class SucursalConfiguration : IEntityTypeConfiguration<SucursalEntity>
    {
        public void Configure(EntityTypeBuilder<SucursalEntity> builder)
        {
            builder.ToTable("SUCURSAL", "booking");
            builder.HasKey(e => e.IdSucursal);

            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal").ValueGeneratedOnAdd();
            builder.Property(e => e.SucursalGuid).HasColumnName("sucursal_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.CodigoSucursal).HasColumnName("codigo_sucursal").HasMaxLength(30);
            builder.Property(e => e.NombreSucursal).HasColumnName("nombre_sucursal").HasMaxLength(150);
            builder.Property(e => e.DescripcionSucursal).HasColumnName("descripcion_sucursal");
            builder.Property(e => e.DescripcionCorta).HasColumnName("descripcion_corta").HasMaxLength(250);
            builder.Property(e => e.TipoAlojamiento).HasColumnName("tipo_alojamiento").HasMaxLength(20);
            builder.Property(e => e.Estrellas).HasColumnName("estrellas");
            builder.Property(e => e.CategoriaViaje).HasColumnName("categoria_viaje").HasMaxLength(30);
            builder.Property(e => e.Pais).HasColumnName("pais").HasMaxLength(100);
            builder.Property(e => e.Provincia).HasColumnName("provincia").HasMaxLength(100);
            builder.Property(e => e.Ciudad).HasColumnName("ciudad").HasMaxLength(100);
            builder.Property(e => e.Ubicacion).HasColumnName("ubicacion").HasMaxLength(200);
            builder.Property(e => e.Direccion).HasColumnName("direccion").HasMaxLength(250);
            builder.Property(e => e.CodigoPostal).HasColumnName("codigo_postal").HasMaxLength(20);
            builder.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(30);
            builder.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(150);
            builder.Property(e => e.Latitud).HasColumnName("latitud").HasColumnType("decimal(10,7)");
            builder.Property(e => e.Longitud).HasColumnName("longitud").HasColumnType("decimal(10,7)");
            builder.Property(e => e.HoraCheckin).HasColumnName("hora_checkin").HasMaxLength(5);
            builder.Property(e => e.HoraCheckout).HasColumnName("hora_checkout").HasMaxLength(5);
            builder.Property(e => e.CheckinAnticipado).HasColumnName("checkin_anticipado");
            builder.Property(e => e.CheckoutTardio).HasColumnName("checkout_tardio");
            builder.Property(e => e.AceptaNinos).HasColumnName("acepta_ninos");
            builder.Property(e => e.EdadMinimaHuesped).HasColumnName("edad_minima_huesped");
            builder.Property(e => e.PermiteMascotas).HasColumnName("permite_mascotas");
            builder.Property(e => e.SePermiteFumar).HasColumnName("se_permite_fumar");
            builder.Property(e => e.EstadoSucursal).HasColumnName("estado_sucursal").HasMaxLength(3);
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("sucursales-service");
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.SucursalGuid).IsUnique();
            builder.HasIndex(e => e.CodigoSucursal).IsUnique();
            builder.HasIndex(e => e.NombreSucursal).IsUnique();

            builder.HasCheckConstraint("CHK_SUCURSAL_ESTADO", "[estado_sucursal] IN ('ACT','INA')");
            builder.HasCheckConstraint("CHK_SUCURSAL_TIPO_ALOJAMIENTO",
                "[tipo_alojamiento] IN ('hotel','hostal','apartamento','resort','villa','cabana','hostel')");
            builder.HasCheckConstraint("CHK_SUCURSAL_ESTRELLAS", "[estrellas] IS NULL OR [estrellas] BETWEEN 1 AND 5");
        }
    }
}