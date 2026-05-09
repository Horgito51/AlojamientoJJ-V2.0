using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicio.Hotel.DataAccess.Entities.Reservas;

namespace Servicio.Hotel.DataAccess.Configurations.Reservas
{
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            builder.ToTable("CLIENTES", "booking");
            builder.HasKey(e => e.IdCliente);

            builder.Property(e => e.IdCliente).HasColumnName("id_cliente").ValueGeneratedOnAdd();
            builder.Property(e => e.ClienteGuid).HasColumnName("cliente_guid").ValueGeneratedOnAdd();
            builder.Property(e => e.TipoIdentificacion).HasColumnName("tipo_identificacion").HasMaxLength(20);
            builder.Property(e => e.NumeroIdentificacion).HasColumnName("numero_identificacion").HasMaxLength(30);
            builder.Property(e => e.Nombres).HasColumnName("nombres").HasMaxLength(160);
            builder.Property(e => e.Apellidos).HasColumnName("apellidos").HasMaxLength(160);
            builder.Property(e => e.RazonSocial).HasColumnName("razon_social").HasMaxLength(200);
            builder.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(150);
            builder.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(30);
            builder.Property(e => e.Direccion).HasColumnName("direccion").HasMaxLength(250);
            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(3);
            builder.Property(e => e.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(e => e.CreadoPorUsuario).HasColumnName("creado_por_usuario").HasMaxLength(100).HasDefaultValue("Sistema");
            builder.Property(e => e.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(e => e.ModificadoPorUsuario).HasColumnName("modificado_por_usuario").HasMaxLength(100);
            builder.Property(e => e.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(e => e.ModificacionIp).HasColumnName("modificacion_ip").HasMaxLength(45);
            builder.Property(e => e.ServicioOrigen).HasColumnName("servicio_origen").HasMaxLength(50).HasDefaultValue("clientes-service");
            builder.Property(e => e.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(e => e.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion").HasMaxLength(250);
            builder.Property(e => e.RowVersion).HasColumnName("row_version").IsRowVersion();

            builder.HasIndex(e => e.ClienteGuid).IsUnique();
            builder.HasIndex(e => e.NumeroIdentificacion).IsUnique();
            builder.HasIndex(e => e.Correo).IsUnique();

            builder.HasCheckConstraint("CHK_CLIENTES_ESTADO", "[estado] IN ('ACT','INA')");
        }
    }
}