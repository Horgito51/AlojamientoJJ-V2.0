using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;

namespace Servicio.Hotel.DataAccess.Context
{
    public class ServicioHotelDbContext : DbContext
    {
        public ServicioHotelDbContext(DbContextOptions<ServicioHotelDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidad
        // Seguridad
        public DbSet<UsuarioAppEntity> UsuariosApp { get; set; }
        public DbSet<RolEntity> Roles { get; set; }
        public DbSet<UsuarioRolEntity> UsuariosRoles { get; set; }
        public DbSet<AuditoriaEntity> Auditorias { get; set; }

        // Alojamiento
        public DbSet<SucursalEntity> Sucursales { get; set; }
        public DbSet<TipoHabitacionEntity> TiposHabitacion { get; set; }
        public DbSet<CatalogoServicioEntity> CatalogosServicios { get; set; }
        public DbSet<TipoHabitacionCatalogoEntity> TiposHabitacionCatalogos { get; set; }
        public DbSet<TipoHabitacionImagenEntity> TiposHabitacionImagenes { get; set; }
        public DbSet<HabitacionEntity> Habitaciones { get; set; }
        public DbSet<TarifaEntity> Tarifas { get; set; }

        // Reservas
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<ReservaEntity> Reservas { get; set; }
        public DbSet<ReservaHabitacionEntity> ReservasHabitaciones { get; set; }

        // Hospedaje
        public DbSet<EstadiaEntity> Estadias { get; set; }
        public DbSet<CargoEstadiaEntity> CargosEstadia { get; set; }

        // Facturación
        public DbSet<FacturaEntity> Facturas { get; set; }
        public DbSet<FacturaDetalleEntity> FacturasDetalle { get; set; }
        public DbSet<PagoEntity> Pagos { get; set; }

        // Valoraciones
        public DbSet<ValoracionEntity> Valoraciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones de IEntityTypeConfiguration del ensamblado actual
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServicioHotelDbContext).Assembly);

            // Filtros globales de eliminación lógica (soft delete)
            // Para entidades que tienen la propiedad "es_eliminado"
            modelBuilder.Entity<UsuarioAppEntity>().HasQueryFilter(u => !u.EsEliminado);
            modelBuilder.Entity<RolEntity>().HasQueryFilter(r => !r.EsEliminado);
            modelBuilder.Entity<UsuarioRolEntity>().HasQueryFilter(ur => !ur.EsEliminado);
            modelBuilder.Entity<AuditoriaEntity>().HasQueryFilter(a => a.Activo); // Auditoria usa Activo
            modelBuilder.Entity<SucursalEntity>().HasQueryFilter(s => !s.EsEliminado);
            modelBuilder.Entity<TipoHabitacionEntity>().HasQueryFilter(th => !th.EsEliminado);
            modelBuilder.Entity<CatalogoServicioEntity>().HasQueryFilter(cs => !cs.EsEliminado);
            // TipoHabitacionCatalogoEntity no tiene es_eliminado, no se aplica filtro
            // TipoHabitacionImagenEntity no tiene es_eliminado
            modelBuilder.Entity<HabitacionEntity>().HasQueryFilter(h => !h.EsEliminado);
            modelBuilder.Entity<TarifaEntity>().HasQueryFilter(t => !t.EsEliminado);
            modelBuilder.Entity<ClienteEntity>().HasQueryFilter(c => !c.EsEliminado);
            modelBuilder.Entity<ReservaEntity>().HasQueryFilter(r => !r.EsEliminado);
            // ReservaHabitacionEntity no tiene es_eliminado
            modelBuilder.Entity<EstadiaEntity>(); // no tiene es_eliminado
            modelBuilder.Entity<CargoEstadiaEntity>(); // no tiene es_eliminado
            modelBuilder.Entity<FacturaEntity>().HasQueryFilter(f => !f.EsEliminado);
            // FacturaDetalleEntity no tiene es_eliminado
            modelBuilder.Entity<PagoEntity>(); // no tiene es_eliminado
            modelBuilder.Entity<ValoracionEntity>(); // no tiene es_eliminado

            // Nota: Para las entidades que no tienen filtro global, se puede agregar si se desea.
        }
    }
}