using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.API.Models.Settings;
using Servicio.Hotel.API.Services;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Facturacion;
using Servicio.Hotel.Business.Interfaces.Hospedaje;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using Servicio.Hotel.Business.Interfaces.Valoraciones;
using Servicio.Hotel.Business.Services.Alojamiento;
using Servicio.Hotel.Business.Services.Facturacion;
using Servicio.Hotel.Business.Services.Hospedaje;
using Servicio.Hotel.Business.Services.Reservas;
using Servicio.Hotel.Business.Services.Seguridad;
using Servicio.Hotel.Business.Services.Valoraciones;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Repositories.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Facturacion;
using Servicio.Hotel.DataAccess.Repositories.Hospedaje;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Hospedaje;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Valoraciones;
using Servicio.Hotel.DataAccess.Repositories.Reservas;
using Servicio.Hotel.DataAccess.Repositories.Seguridad;
using Servicio.Hotel.DataAccess.Repositories.Valoraciones;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;
using Servicio.Hotel.DataManagement.Alojamiento.Services;
using Servicio.Hotel.DataManagement.Facturacion.Interfaces;
using Servicio.Hotel.DataManagement.Facturacion.Services;
using Servicio.Hotel.DataManagement.Hospedaje.Interfaces;
using Servicio.Hotel.DataManagement.Hospedaje.Services;
using Servicio.Hotel.DataManagement.Reservas.Interfaces;
using Servicio.Hotel.DataManagement.Reservas.Services;
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;
using Servicio.Hotel.DataManagement.Seguridad.Services;
using Servicio.Hotel.DataManagement.UnitOfWork;
using Servicio.Hotel.DataManagement.Valoraciones.Interfaces;
using Servicio.Hotel.DataManagement.Valoraciones.Services;

namespace Servicio.Hotel.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ServicioHotelDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddOptions<PaymentGatewaySettings>()
                .BindConfiguration("PaymentGateway");
            services.AddHttpClient<IPaymentGateway, HttpPaymentGateway>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositorios
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IReservaHabitacionRepository, ReservaHabitacionRepository>();
            services.AddScoped<IEstadiaRepository, EstadiaRepository>();
            services.AddScoped<ICargoEstadiaRepository, CargoEstadiaRepository>();
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            services.AddScoped<IPagoRepository, PagoRepository>();
            services.AddScoped<ITipoHabitacionRepository, TipoHabitacionRepository>();
            services.AddScoped<ITarifaRepository, TarifaRepository>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<ISucursalRepository, SucursalRepository>();
            services.AddScoped<ICatalogoServicioRepository, CatalogoServicioRepository>();
            services.AddScoped<IValoracionRepository, ValoracionRepository>();
            services.AddScoped<IUsuarioAppRepository, UsuarioAppRepository>();
            services.AddScoped<IRolRepository, RolRepository>();

            // Capa de datos (servicios sobre repositorios)
            services.AddScoped<IClienteDataService, ClienteDataService>();
            services.AddScoped<IReservaDataService, ReservaDataService>();
            services.AddScoped<IEstadiaDataService, EstadiaDataService>();
            services.AddScoped<IFacturaDataService, FacturaDataService>();
            services.AddScoped<IPagoDataService, PagoDataService>();
            services.AddScoped<ITipoHabitacionDataService, TipoHabitacionDataService>();
            services.AddScoped<ITarifaDataService, TarifaDataService>();
            services.AddScoped<IHabitacionDataService, HabitacionDataService>();
            services.AddScoped<ISucursalDataService, SucursalDataService>();
            services.AddScoped<ICatalogoServicioDataService, CatalogoServicioDataService>();
            services.AddScoped<IValoracionDataService, ValoracionDataService>();
            services.AddScoped<IUsuarioDataService, UsuarioDataService>();
            services.AddScoped<IRolDataService, RolDataService>();

            // Lógica de negocio
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IEstadiaService, EstadiaService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<IPagoService, PagoService>();
            services.AddScoped<ITipoHabitacionService, TipoHabitacionService>();
            services.AddScoped<ITarifaService, TarifaService>();
            services.AddScoped<IHabitacionService, HabitacionService>();
            services.AddScoped<ISucursalService, SucursalService>();
            services.AddScoped<ICatalogoServicioService, CatalogoServicioService>();
            services.AddScoped<IValoracionService, ValoracionService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IRolService, RolService>();

            return services;
        }
    }
}
