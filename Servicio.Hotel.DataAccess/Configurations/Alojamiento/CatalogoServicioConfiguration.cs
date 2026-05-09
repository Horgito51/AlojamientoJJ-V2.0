using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Configurations.Alojamiento
{
    public class CatalogoServicioConfiguration : IEntityTypeConfiguration<CatalogoServicioEntity>
    {
        public void Configure(EntityTypeBuilder<CatalogoServicioEntity> builder)
        {
            builder.ToTable("CATALOGO_SERVICIOS", "booking");
            builder.HasKey(e => e.IdCatalogo);

            builder.Property(e => e.IdCatalogo).HasColumnName("id_catalogo").ValueGeneratedOnAdd();
            builder.Property(e => e.CatalogoGuid).HasColumnName("catalogo_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            builder.Property(e => e.CodigoCatalogo).HasColumnName("codigo_catalogo").HasMaxLength(30);
            builder.Property(e => e.NombreCatalogo).HasColumnName("nombre_catalogo").HasMaxLength(120);
            builder.Property(e => e.TipoCatalogo).HasColumnName("tipo_catalogo").HasMaxLength(3);
            builder.Property(e => e.CategoriaCatalogo).HasColumnName("categoria_catalogo").HasMaxLength(80);
            builder.Property(e => e.DescripcionCatalogo).HasColumnName("descripcion_catalogo").HasMaxLength(250);
            builder.Property(e => e.PrecioBase).HasColumnName("precio_base").HasColumnType("decimal(12,2)");
            builder.Property(e => e.AplicaIva).HasColumnName("aplica_iva");
            builder.Property(e => e.Disponible24h).HasColumnName("disponible_24h");
            builder.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            builder.Property(e => e.HoraFin).HasColumnName("hora_fin");
            builder.Property(e => e.IconoUrl).HasColumnName("icono_url").HasMaxLength(500);
            builder.Property(e => e.EstadoCatalogo).HasColumnName("estado_catalogo").HasMaxLength(3);
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

            builder.HasIndex(e => e.CatalogoGuid).IsUnique();
            builder.HasIndex(e => e.CodigoCatalogo).IsUnique();

            builder.HasOne(e => e.Sucursal)
                .WithMany(s => s.CatalogosServicios)
                .HasForeignKey(e => e.IdSucursal);

            builder.HasCheckConstraint("CHK_CATALOGO_TIPO", "[tipo_catalogo] IN ('AME','SRV')");
            builder.HasCheckConstraint("CHK_CATALOGO_ESTADO", "[estado_catalogo] IN ('ACT','INA')");
            builder.HasCheckConstraint("CHK_CATALOGO_PRECIO",
                "([tipo_catalogo] = 'AME' AND [precio_base] = 0) OR ([tipo_catalogo] = 'SRV' AND [precio_base] >= 0)");
        }
    }
}