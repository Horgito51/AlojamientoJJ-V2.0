# ARCHITECTURE_CONTEXT.md

## 1. Propósito del documento

Este documento es la memoria técnica permanente del microservicio hotelero `Servicio.Hotel`. Está diseñado para que herramientas de IA, Codex, ChatGPT, asistentes de programación y nuevos desarrolladores puedan entender la arquitectura, las reglas de negocio, las convenciones y las restricciones del sistema sin necesidad de reexplicar el proyecto.

Debe usarse antes de implementar cualquier cambio funcional, endpoint, refactor, integración externa o ajuste de persistencia.

El objetivo principal es evitar regresiones arquitectónicas como:

- Acceso directo a base de datos desde controladores.
- Lógica de negocio dentro de la capa API.
- Exposición de identificadores internos en contratos públicos.
- Duplicación de reglas existentes de reservas, disponibilidad, tarifas, seguridad o facturación.
- Mezcla de modelos de EF Core con DTOs de negocio o contratos HTTP.
- Cambios inconsistentes entre API pública, API interna, Business, DataManagement y DataAccess.

## 2. Visión general del sistema

`Servicio.Hotel` es una API hotelera orientada a gestión de alojamiento, reservas y operación hotelera. El sistema administra sucursales hoteleras, habitaciones, tipos de habitación, tarifas, clientes, reservas, estadías, facturas, pagos, valoraciones y autenticación.

El sistema está en un proceso de evolución desde una arquitectura monolítica en capas hacia una arquitectura más modular, preparada para microservicios. Actualmente todos los dominios conviven dentro de una misma solución, pero el código ya separa responsabilidades por proyectos, interfaces, servicios y módulos de dominio.

Dominios funcionales principales:

- `Alojamiento`: sucursales, habitaciones, tipos de habitación, tarifas y catálogo de servicios.
- `Reservas`: clientes, reservas, detalles de habitaciones reservadas, estados y disponibilidad.
- `Hospedaje`: estadías, check-in, check-out y cargos durante la estadía.
- `Facturación`: facturas, detalles de factura, pagos y pasarela de pagos.
- `Seguridad`: usuarios, roles, login, refresh token, JWT, cambio de contraseña y asociación usuario-cliente.
- `Valoraciones`: reviews de clientes, puntuaciones, moderación y publicación en portal.
- `Booking/Public`: contratos públicos estilo marketplace, pensados para integraciones externas y canales tipo Booking.

El sistema expone dos familias de API:

- API interna/backoffice: orientada a administración hotelera, recepción, operación y procesos internos. Puede usar IDs numéricos internos.
- API pública/marketplace: orientada a clientes, integraciones externas y canales de reserva. Debe usar GUIDs y contratos públicos separados.

## 3. Estado tecnológico actual

El contexto funcional del proyecto indica .NET 8 y ASP.NET Core Web API. Sin embargo, los archivos `.csproj` actuales apuntan a:

- `TargetFramework`: `net10.0`.
- Paquetes ASP.NET Core/EF Core en versiones `10.x`.
- `System.IdentityModel.Tokens.Jwt` versión `8.6.0`.
- `Asp.Versioning.Mvc` versión `8.1.1`.
- `Swashbuckle.AspNetCore` versión `6.6.2`.

Regla para futuras implementaciones:

- No asumir automáticamente .NET 8 al instalar paquetes o modificar configuración.
- Verificar el `TargetFramework` real antes de cambiar dependencias.
- Si el objetivo institucional es volver a .NET 8, debe hacerse como una migración explícita y coordinada, no como cambio incidental.

## 4. Estructura de la solución

La solución está compuesta por cuatro proyectos principales:

```text
Servicio.Hotel
├── Servicio.Hotel.API
├── Servicio.Hotel.Business
├── Servicio.Hotel.DataManagement
└── Servicio.Hotel.DataAccess
```

El flujo arquitectónico esperado es:

```text
HTTP Request
  -> Controller / Middleware / Filter
  -> Business Service
  -> DataManagement Service
  -> Repository / Query Repository
  -> DbContext / SQL Server / Stored Procedure
  -> Repository
  -> DataManagement Mapper
  -> Business Mapper
  -> Controller Mapper / Response DTO
  -> HTTP Response
```

No se deben saltar capas.

## 5. Servicio.Hotel.API

### Responsabilidad

`Servicio.Hotel.API` es la capa de entrada HTTP. Su responsabilidad es recibir requests, aplicar autenticación/autorización, mapear contratos HTTP hacia DTOs de negocio, devolver responses y manejar concerns propios de transporte.

Debe contener:

- Controllers.
- Request contracts.
- Response contracts públicos/internos cuando apliquen.
- Middlewares.
- Filters.
- Configuración de Swagger.
- Configuración de CORS.
- Configuración de API Versioning.
- Configuración de JWT Bearer.
- Registro de dependencias.
- Adaptadores de infraestructura externa necesarios por composición, por ejemplo `HttpPaymentGateway`.

No debe contener:

- Reglas de negocio.
- Queries directas a EF Core.
- Uso directo de `ServicioHotelDbContext`.
- Cálculo de disponibilidad.
- Cálculo de IVA, totales o descuentos.
- Validaciones profundas de dominio que ya pertenezcan a `Business`.
- Mapeo de entidades EF a responses.

### Estructura relevante

```text
Servicio.Hotel.API
├── Authorization
│   └── AuthorizationPolicies.cs
├── Controllers
│   └── V1
│       ├── Auth
│       ├── Booking
│       └── Internal
├── Extensions
│   ├── ApiVersioningExtensions.cs
│   ├── AuthenticationExtensions.cs
│   ├── AuthorizationExtensions.cs
│   ├── CorsExtensions.cs
│   ├── ServiceCollectionExtensions.cs
│   └── SwaggerExtensions.cs
├── Filters
│   └── ValidationFilter.cs
├── Middleware
│   ├── AdminProfileAccessMiddleware.cs
│   └── ExceptionHandlingMiddleware.cs
├── Models
│   ├── Common
│   ├── Requests
│   │   ├── Internal
│   │   └── Public
│   ├── Responses
│   │   ├── Internal
│   │   └── Public
│   └── Settings
└── Services
    └── HttpPaymentGateway.cs
```

### Controllers internos

Los controladores internos se ubican bajo:

```text
Servicio.Hotel.API/Controllers/V1/Internal
```

Rutas esperadas:

```text
/api/v{version}/internal/{recurso}
```

Ejemplos:

- `/api/v1/internal/reservas`
- `/api/v1/internal/clientes`
- `/api/v1/internal/habitaciones`
- `/api/v1/internal/sucursales`
- `/api/v1/internal/tarifas`
- `/api/v1/internal/facturas`
- `/api/v1/internal/pagos`
- `/api/v1/internal/valoraciones`

Los endpoints internos pueden aceptar IDs numéricos porque son contratos de administración/backoffice.

### Controllers públicos/Booking

Los controladores públicos se ubican bajo:

```text
Servicio.Hotel.API/Controllers/V1/Booking
```

Rutas existentes relevantes:

- `/api/v1/accommodations/search`
- `/api/v1/accommodations/{habitacionGuid}`
- `/api/v1/accommodations/categories`
- `/api/v1/accommodations/{habitacionGuid}/reviews`
- `/api/v1/public/reservas`
- `/api/v1/public/reservas/{reservaGuid}`
- `/api/v1/public/reservas/calcular-precio`

Regla crítica:

- La API pública debe usar GUIDs.
- La API pública no debe aceptar IDs numéricos internos.
- `PublicRequestGuard` rechaza propiedades tipo `id`, prefijos `id...` o sufijos `...id` en payloads públicos.

## 6. Servicio.Hotel.Business

### Responsabilidad

`Servicio.Hotel.Business` es la capa de reglas de negocio. Representa el núcleo aplicativo del sistema. Contiene DTOs de negocio, interfaces de servicios, implementaciones de servicios, validadores, excepciones de negocio, mappers y utilidades comunes.

Debe contener:

- Reglas de negocio.
- Validaciones de dominio.
- Orquestación entre servicios de dominio.
- Cálculo de precios, IVA, descuentos y saldos.
- Verificación de disponibilidad.
- Reglas de cambio de estado.
- Excepciones de negocio estandarizadas.
- DTOs de negocio.
- Interfaces públicas de caso de uso.

No debe contener:

- Controllers.
- Tipos HTTP (`HttpContext`, `ActionResult`, `IActionResult`).
- Dependencia directa de EF Core.
- Entidades de base de datos como contrato externo.
- SQL raw.
- Stored procedures directos.

### Estructura relevante

```text
Servicio.Hotel.Business
├── Common
├── DTOs
│   ├── Alojamiento
│   ├── Booking
│   ├── Facturacion
│   ├── Hospedaje
│   ├── Reservas
│   ├── Seguridad
│   └── Valoraciones
├── Exceptions
├── Interfaces
│   ├── Alojamiento
│   ├── Facturacion
│   ├── Hospedaje
│   ├── Reservas
│   ├── Seguridad
│   └── Valoraciones
├── Mappers
├── Services
└── Validators
```

### Servicios de negocio

Los servicios de negocio coordinan casos de uso. Ejemplos:

- `ReservaService`
- `ClienteService`
- `HabitacionService`
- `TarifaService`
- `SucursalService`
- `FacturaService`
- `PagoService`
- `AuthService`
- `UsuarioService`
- `RolService`
- `ValoracionService`

Regla de diseño:

- Un service puede llamar a otros services cuando necesita reglas de otro dominio.
- Un service no debe hablar directamente con repositories.
- Un service debe depender de interfaces (`IReservaDataService`, `ITarifaService`, etc.).
- Las reglas de negocio que se reutilizan en varios endpoints deben vivir aquí, no en controllers.

Ejemplo real:

- `ReservaService.CreateAsync` calcula precios por habitación, IVA, subtotal, total, saldo pendiente, valida reserva y verifica disponibilidad antes de persistir.
- `ReservaService.ConfirmarAsync` valida estado, disponibilidad final y ejecuta confirmación más generación de factura dentro de una transacción.
- `AuthService` valida credenciales, genera access tokens, refresh tokens, claims y reglas de usuario activo.

## 7. Servicio.Hotel.DataManagement

### Responsabilidad

`Servicio.Hotel.DataManagement` es una capa de aplicación de persistencia y coordinación de datos. Actúa como puente entre `Business` y `DataAccess`.

Debe contener:

- Data services.
- Data models.
- Data mappers.
- Interfaces de data services.
- Unit of Work.
- Excepciones de dominio relacionadas con persistencia.
- Coordinación de múltiples repositories.
- Transacciones sobre `DbContext`.
- Uso controlado de `ServicioHotelDbContext` cuando se requiere coordinación o consultas agregadas.

No debe contener:

- Controllers.
- Contratos HTTP.
- Claims/JWT.
- Decisiones de autorización.
- Reglas de negocio de alto nivel que pertenezcan a `Business`.

### Rol en la arquitectura actual

Esta capa no es un repository. Es un nivel de coordinación sobre repositories y EF Core. Por ejemplo:

- Valida existencia de entidades relacionadas antes de insertar.
- Ejecuta operaciones dentro de `IUnitOfWork`.
- Mapea `Entity` <-> `DataModel`.
- Llama repositories específicos.
- En algunos casos coordina stored procedures mediante repositories.

Ejemplo real:

- `ReservaDataService.AddAsync` crea cabecera de reserva, guarda cambios, inserta detalles vía `SP_CONFIRMAR_RESERVA_HABITACION`, recalcula totales y limpia tracking de EF Core.

Esta lógica no debe copiarse en un controller ni en un endpoint público.

## 8. Servicio.Hotel.DataAccess

### Responsabilidad

`Servicio.Hotel.DataAccess` es la capa de infraestructura de persistencia. Contiene EF Core, entities, configuraciones de mapeo, repositories y query repositories.

Debe contener:

- `ServicioHotelDbContext`.
- `DbSet<T>`.
- Entities.
- Configurations de EF Core.
- Repositories.
- Interfaces de repositories.
- Query repositories especializados.
- Acceso a stored procedures.
- SQL raw cuando sea estrictamente necesario.

No debe contener:

- DTOs de API.
- DTOs de Business.
- Reglas de negocio de alto nivel.
- Autenticación HTTP.
- Validaciones de controllers.

### DbContext

`ServicioHotelDbContext` define `DbSet` para:

- Seguridad: `UsuariosApp`, `Roles`, `UsuariosRoles`, `Auditorias`.
- Alojamiento: `Sucursales`, `TiposHabitacion`, `CatalogosServicios`, `TiposHabitacionCatalogos`, `TiposHabitacionImagenes`, `Habitaciones`, `Tarifas`.
- Reservas: `Clientes`, `Reservas`, `ReservasHabitaciones`.
- Hospedaje: `Estadias`, `CargosEstadia`.
- Facturación: `Facturas`, `FacturasDetalle`, `Pagos`.
- Valoraciones: `Valoraciones`.

También aplica configuraciones mediante:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServicioHotelDbContext).Assembly);
```

Y define filtros globales de soft delete para varias entidades:

- Usuarios.
- Roles.
- Sucursales.
- Tipos de habitación.
- Catálogo de servicios.
- Habitaciones.
- Tarifas.
- Clientes.
- Reservas.
- Facturas.

Regla:

- Antes de implementar consultas administrativas que deban incluir eliminados, considerar que existen query filters globales.
- No usar `IgnoreQueryFilters()` sin justificar el caso de uso.

## 9. Flujo completo de una request

### Flujo nominal

```text
Cliente HTTP
  -> ASP.NET Core Middleware Pipeline
  -> ExceptionHandlingMiddleware
  -> CORS
  -> Authentication
  -> AdminProfileAccessMiddleware
  -> Authorization
  -> Controller
  -> Request contract / mapper
  -> Business service
  -> Business validator
  -> DataManagement service
  -> Repository / QueryRepository
  -> ServicioHotelDbContext
  -> SQL Server
  -> Entity
  -> DataModel
  -> Business DTO
  -> Response DTO
  -> HTTP status code
```

### Ejemplo: crear reserva pública

1. `POST /api/v1/public/reservas` recibe `PublicReservaCreateRequest`.
2. El controller ejecuta `ValidateNoIds()` para impedir IDs numéricos.
3. El controller valida campos mínimos de transporte: fechas, GUIDs obligatorios y habitaciones.
4. Se resuelve `ClienteGuid` a `IdCliente` usando `IClienteService`.
5. Se resuelve `SucursalGuid` a `IdSucursal` usando `ISucursalService`.
6. Se resuelve cada `HabitacionGuid` a `IdHabitacion` usando `IHabitacionService`.
7. El controller construye `ReservaCreateDTO`.
8. `ReservaService.CreateAsync` calcula precios, IVA, totales y saldo.
9. `ReservaService` valida reglas de negocio con `ReservaValidator`.
10. `ReservaService` verifica disponibilidad y estado reservable de habitaciones.
11. `ReservaDataService.AddAsync` coordina persistencia transaccional.
12. `ReservaRepository` usa EF Core y stored procedure para detalles.
13. Se recarga la reserva creada.
14. El controller transforma `ReservaDTO` a `ReservaPublicDto`.
15. Se retorna `201 Created`.

### Ejemplo: confirmar reserva interna

1. `PATCH /api/v1/internal/reservas/{id}/confirmar`.
2. Controller llama `IReservaService.ConfirmarAsync`.
3. `ReservaService` obtiene reserva.
4. Valida existencia.
5. Valida estado `PEN`.
6. Valida que tenga habitaciones.
7. Verifica que cada habitación siga reservable.
8. Verifica solapamientos activos.
9. Ejecuta transacción:
   - Confirmar reserva.
   - Generar factura de reserva.
10. Retorna `204 NoContent`.

## 10. Principios arquitectónicos

### Clean Architecture aplicada

El sistema sigue una variante pragmática de Clean Architecture con arquitectura en capas.

Dirección deseada de dependencias:

```text
API -> Business -> DataManagement -> DataAccess
```

Aunque `API` actualmente referencia también `DataManagement` y `DataAccess` para composición de dependencias, esa referencia debe usarse únicamente en el punto de arranque (`Program.cs`, extensions de DI). Los controllers no deben usar esas capas directamente.

### Separación de responsabilidades

Cada capa tiene un propósito concreto:

- API: transporte HTTP.
- Business: reglas de negocio.
- DataManagement: coordinación de persistencia y transacciones.
- DataAccess: EF Core, SQL Server, repositories y entities.

### SOLID

Reglas prácticas:

- Single Responsibility: un controller maneja endpoints, no reglas complejas.
- Open/Closed: extender services/mappers antes de modificar flujos ajenos.
- Liskov: respetar contratos de interfaces existentes.
- Interface Segregation: crear interfaces por dominio, no interfaces gigantes.
- Dependency Inversion: depender de interfaces, no implementaciones concretas.

### DTO mapping

El sistema usa varias familias de modelos:

```text
Request DTO/API Contract
  -> Business DTO
  -> DataModel
  -> Entity
```

Y de retorno:

```text
Entity
  -> DataModel
  -> Business DTO
  -> Response DTO/API Contract
```

No mezclar estos niveles.

## 11. Reglas obligatorias para futuras implementaciones

### Reglas de capas

- No acceder a base de datos desde controllers.
- No inyectar `ServicioHotelDbContext` en controllers.
- No inyectar repositories en controllers.
- No colocar lógica de negocio en API.
- No devolver entities EF Core desde endpoints.
- No recibir entities EF Core como payload HTTP.
- No exponer `DataModel` como contrato HTTP.
- No duplicar lógica existente de services.
- No crear queries ad hoc en controllers.
- No llamar stored procedures desde Business ni API.
- No crear transacciones manuales fuera de `IUnitOfWork`.

### Reglas de Business

- Validaciones de negocio deben vivir en validators o services de Business.
- Cambios de estado deben validarse en Business.
- Cálculo de precios debe pasar por `ReservaService.CalcularPrecioHabitacionAsync`.
- Disponibilidad debe validarse con reglas existentes antes de confirmar o crear reservas.
- Excepciones deben usar los tipos existentes: `ValidationException`, `NotFoundException`, `ConflictException`, `BusinessException`, `UnauthorizedBusinessException`.

### Reglas de DataManagement

- Data services coordinan repositories.
- Data services convierten entities a data models.
- Data services pueden usar `IUnitOfWork`.
- Data services pueden usar `DbContext` para coordinación o consultas agregadas cuando sea necesario.
- No deben conocer contratos HTTP.

### Reglas de DataAccess

- Repositories encapsulan EF Core.
- Query repositories deben usarse para consultas complejas o optimizadas.
- Stored procedures pertenecen a repositories.
- Entities pertenecen exclusivamente a DataAccess.
- Configuraciones de tablas, columnas, constraints y relaciones pertenecen a `Configurations`.

### Reglas de naming y consistencia

- Mantener sufijos actuales:
  - `Controller`
  - `Service`
  - `DataService`
  - `Repository`
  - `DTO`
  - `DataModel`
  - `Entity`
  - `Validator`
  - `Mapper`
  - `Request`
  - `Response`
- Mantener namespaces alineados por dominio.
- Usar nombres en español para dominios internos existentes.
- Usar nombres públicos consistentes con marketplace cuando el contrato sea externo.

## 12. DTOs y contratos

### Request DTOs

Los request DTOs viven en:

```text
Servicio.Hotel.API/Models/Requests
```

Familias:

- `Internal`: contratos internos/backoffice.
- `Public`: contratos públicos/marketplace.

Los request DTOs deben:

- Representar exactamente el payload HTTP.
- No incluir campos calculados si el backend debe calcularlos.
- No incluir campos auditables de escritura interna salvo caso justificado.
- No mapearse directamente a entities.

### Response DTOs

Los response DTOs viven en:

```text
Servicio.Hotel.API/Models/Responses
```

Familias:

- `Internal`: responses específicos internos cuando se requiera separar del Business DTO.
- `Public`: responses públicos que deben ocultar IDs internos.

### Business DTOs

Los DTOs de Business viven en:

```text
Servicio.Hotel.Business/DTOs
```

Representan conceptos de negocio y casos de uso. No son contratos HTTP puros, aunque algunos controllers internos todavía los retornan directamente en ciertos endpoints. La dirección recomendada es separar cada vez más:

- `CreateDTO`.
- `UpdateDTO`.
- `ResponseDTO`.
- `FiltroDTO`.
- DTOs de comando para cambios de estado.

### DataModels

Los DataModels viven en:

```text
Servicio.Hotel.DataManagement/{Dominio}/Models
```

Representan datos transferidos entre DataManagement y Business. No deben salir por HTTP.

### Entities

Las entities viven en:

```text
Servicio.Hotel.DataAccess/Entities
```

Representan tablas, columnas, relaciones y persistencia EF Core. Nunca deben exponerse en API.

### DTOs externos tipo Booking

Los contratos públicos tipo Booking deben:

- Usar GUIDs.
- Evitar IDs numéricos.
- Tener nombres estables y orientados a integradores.
- Ocultar detalles internos como `IdReserva`, `IdCliente`, `IdHabitacion`, `RowVersion` o flags técnicos.
- Mapear hacia DTOs internos mediante servicios existentes.
- Reutilizar Business para reglas de negocio.

## 13. JWT y seguridad

### Configuración actual

JWT se configura en `AuthenticationExtensions.AddJwtAuthentication`.

Validaciones:

- Secret obligatorio.
- Secret mínimo de 32 caracteres.
- Issuer válido.
- Audience válido.
- Firma HMAC SHA256.
- Lifetime validado.
- `ClockSkew = TimeSpan.Zero`.
- `RequireHttpsMetadata = !environment.IsDevelopment()`.

Los errores de autenticación se devuelven con `ApiErrorResponse` y status `401`.

### Generación de tokens

`AuthService` genera:

- Access token.
- Refresh token.

Claims relevantes del access token:

- `ClaimTypes.NameIdentifier`: ID interno de usuario.
- `ClaimTypes.Name`: username.
- `ClaimTypes.Email`: correo.
- `usuarioGuid`: GUID público del usuario.
- `nombres`.
- `apellidos`.
- `idCliente`: si el usuario está asociado a un cliente.
- `ClaimTypes.Role`: uno por cada rol.

Claims relevantes del refresh token:

- `ClaimTypes.NameIdentifier`.
- `ClaimTypes.Name`.
- `token_type = refresh`.

### Roles y políticas

`AuthorizationPolicies` define:

- `AdminProfile`.
- `BackOffice`.
- `ClienteRole = CLIENTE`.
- Roles backoffice:
  - `ADMINISTRADOR`
  - `ADMIN`
  - `RECEPCIONISTA`
  - `OPERATIVO`
  - `DESK_SERVICE`

Reglas:

- Proteger endpoints internos con `[Authorize]`.
- Usar policies cuando el endpoint sea administrativo.
- No confiar únicamente en que el frontend o marketplace filtre permisos.
- Validar ownership en Business o en el controller usando services cuando un cliente accede a sus propios recursos.

### Ownership validation

Para endpoints públicos autenticados, no basta con tener token válido. Debe verificarse que el recurso pertenece al cliente autenticado.

Patrón actual:

- Leer `idCliente` desde claim si existe.
- Si no existe, resolver usuario por `ClaimTypes.NameIdentifier`.
- Si el usuario tiene `IdCliente`, usarlo.
- Si no tiene cliente, crear cliente asociado y persistir asociación.

Regla futura:

- Para consultar, cancelar, pagar o valorar una reserva pública, verificar que `reserva.IdCliente` coincide con el cliente autenticado, salvo endpoints explícitamente anónimos.

### Seguridad entre microservicios futura

Cuando el sistema se divida:

- Usar un servicio central de Auth/Identity.
- Validar JWT en cada microservicio.
- Evaluar tokens internos de servicio a servicio con audience específica.
- No reutilizar tokens de usuario para procesos backend sin validar scopes.
- Considerar mTLS o client credentials para comunicación interna.
- Incluir correlation ID y tenant/sucursal si aplica.

## 14. Endpoints públicos vs internos

### Endpoints públicos

Características:

- Orientados a marketplace, clientes y canales externos.
- Usan GUIDs.
- Deben ocultar IDs numéricos.
- Pueden tener contratos diferentes a los DTOs internos.
- Deben ser estables y backward compatible.
- Deben tener validación estricta de propiedades no permitidas.
- No deben exponer campos administrativos.

Ejemplos:

- `HabitacionPublicDto`
- `ReservaPublicDto`
- `ReservaPrecioPublicDto`
- `PublicReservaCreateRequest`
- `PublicPagoSimularRequest`

### Endpoints internos

Características:

- Orientados a administración/backoffice.
- Pueden usar IDs numéricos.
- Pueden exponer más detalle operativo.
- Requieren autenticación.
- Deben considerar roles/policies.

Ejemplos:

- `ReservaCreateRequest`.
- `PagoCreateRequest`.
- `ValoracionModeracionRequest`.
- `SucursalUpsertRequest`.

### Por qué existen contratos diferentes

Los contratos públicos e internos tienen objetivos distintos:

- El contrato público protege el core interno, oculta IDs y ofrece estabilidad a integradores.
- El contrato interno prioriza operación administrativa y trazabilidad.
- El core de negocio debe ser reutilizado por ambos.

No crear dos lógicas de reserva distintas para público e interno. Crear dos adaptadores HTTP diferentes que llamen al mismo `IReservaService`.

## 15. Manejo de reservas

### Entidades/conceptos principales

- `Reserva`: cabecera de reserva.
- `ReservaHabitacion`: detalle por habitación reservada.
- `Cliente`: titular de la reserva.
- `Sucursal`: hotel/sucursal.
- `Habitacion`: unidad reservable.
- `Tarifa`: precio vigente por sucursal, tipo de habitación, rango y canal.
- `Factura`: documento generado al confirmar.
- `Pago`: pagos asociados.

### Estados relevantes

Estados vistos en reglas y constraints:

- `PEN`: pendiente.
- `CON`: confirmada.
- `CAN`: cancelada.
- `EXP`: expirada.
- `FIN`: finalizada.
- `EMI`: emitida o estado relacionado a facturación/estadía según flujo actual.

Reglas actuales:

- Una reserva `CAN` o `FIN` no debe modificarse.
- Solo una reserva `PEN` puede confirmarse.
- Una reserva `PEN` o `CON` puede cancelarse.
- La cancelación requiere motivo.
- La confirmación genera factura en transacción.

### Flujo de creación

1. Validar que existe al menos una habitación.
2. Para cada habitación:
   - Resolver habitación.
   - Validar estado reservable.
   - Resolver tarifa vigente o usar precio base.
   - Calcular noches.
   - Calcular subtotal.
   - Calcular IVA.
   - Calcular total de línea.
3. Calcular subtotal total.
4. Calcular IVA de reserva.
5. Calcular total menos descuento.
6. Calcular saldo pendiente.
7. Validar DTO con `ReservaValidator`.
8. Verificar que las habitaciones pertenecen a la sucursal.
9. Verificar solapamientos de fechas.
10. Persistir cabecera y detalles.

### Cálculo de IVA

Regla actual en reservas:

- IVA fijo de `0.12m`.
- Se redondea con `Math.Round(..., 2)`.
- Línea:
  - `SubtotalLinea = PrecioNocheAplicado * noches`.
  - `ValorIvaLinea = SubtotalLinea * 0.12`.
  - `TotalLinea = SubtotalLinea + ValorIvaLinea`.
- Cabecera:
  - `ValorIva = SubtotalReserva * 0.12`.
  - `TotalReserva = SubtotalReserva + ValorIva - DescuentoAplicado`.

Recomendación:

- Si el IVA debe variar por tarifa o país, centralizarlo en un servicio de pricing/tax futuro. No dispersar constantes.

### Descuentos

Campos actuales:

- `DescuentoAplicado` en cabecera.
- `DescuentoLinea` en detalle.

Reglas:

- No permitir descuentos negativos.
- No permitir total final menor a cero.
- Documentar si el descuento aplica antes o después del IVA antes de cambiar cálculos.

### Transacciones

Usar `IUnitOfWork.ExecuteInTransactionAsync` para operaciones compuestas:

- Crear reserva con detalles.
- Confirmar reserva y generar factura.
- Registrar pago y actualizar saldo/estado.
- Check-in/check-out con cargos.

No abrir transacciones en controllers.

## 16. Disponibilidad hotelera

### Definición

Una habitación está disponible para un rango si:

- Existe.
- Pertenece a la sucursal solicitada.
- Su estado permite reserva.
- No tiene reservas activas solapadas en el rango.
- El tipo de habitación permite reserva pública cuando el canal es marketplace.
- La tarifa vigente/canal es compatible cuando aplica.

### Estados de habitación no reservables

`ReservaService.EnsureHabitacionReservable` rechaza:

- `MNT`: mantenimiento.
- `FDS`: fuera de servicio.
- `OCU`: ocupada.
- `INA`: inactiva.

Estado normalmente reservable:

- `DIS`: disponible.

### Rangos de fechas

Regla:

- `FechaFin` debe ser posterior a `FechaInicio`.
- Noches = diferencia en días entre `FechaFin.Date` y `FechaInicio.Date`.
- Si el cálculo da `<= 0`, se fuerza a 1 en pricing, pero validaciones deben impedir rangos inválidos.

### Solapamiento

El solapamiento se valida con:

```text
IReservaDataService.ExisteSolapamientoAsync
  -> IReservaHabitacionRepository.ExistsSolapamientoAsync
```

Regla:

- Al actualizar una reserva, excluir la propia reserva mediante `excludeIdReserva`.
- Al confirmar, volver a validar solapamiento para evitar race conditions funcionales.
- Las reservas canceladas/finalizadas no deberían bloquear disponibilidad. Verificar implementación del repository antes de modificar.

### Performance de disponibilidad

Evitar:

- Traer todas las habitaciones y filtrar en memoria para búsquedas grandes.
- Ejecutar N+1 queries por habitación sin necesidad.
- Consultas públicas sin paginación.

Recomendación:

- Crear query repository especializado para disponibilidad por sucursal, fechas, capacidad y canal.
- Usar índices por `IdHabitacion`, `FechaInicio`, `FechaFin`, `EstadoDetalle`, `EstadoReserva`.

## 17. Paginación y filtros

### Estado actual

Existen clases:

- `Servicio.Hotel.Business.Common.PagedResult<T>`.
- `Servicio.Hotel.DataManagement.Common.DataPagedResult<T>`.
- `Servicio.Hotel.DataAccess.Common.Pagination.PagedResult<T>`.

Patrón usual:

```text
FiltroDTO + pageNumber + pageSize
  -> DataService
  -> DataPagedResult<DataModel>
  -> PagedResult<DTO>
```

### Estándar recomendado

Parámetros:

- `page`: número de página, mínimo 1.
- `limit` o `pageSize`: tamaño, mínimo 1, máximo recomendado 100.
- filtros específicos por dominio.

Response recomendado:

```json
{
  "items": [],
  "totalCount": 0,
  "pageNumber": 1,
  "pageSize": 50
}
```

Opcional futuro:

```json
{
  "items": [],
  "pagination": {
    "totalCount": 0,
    "page": 1,
    "pageSize": 50,
    "totalPages": 0,
    "hasNext": false,
    "hasPrevious": false
  }
}
```

### Performance

Reglas:

- Aplicar filtros en base de datos, no en memoria.
- Evitar `GetAllAsync()` seguido de `AsQueryable()` sobre colecciones ya materializadas para datasets grandes.
- Query repositories deben devolver `IQueryable` controlado o resultados paginados.
- Usar `AsNoTracking()` en consultas de solo lectura.
- Evitar incluir relaciones innecesarias.

## 18. Integración con contratos externos

### Objetivo

La API pública debe permitir integraciones tipo Booking, marketplace o frontend público sin contaminar el core interno.

### Patrón correcto

```text
Contrato externo
  -> Controller público
  -> Validación de contrato externo
  -> Resolución GUID -> ID interno
  -> DTO de negocio
  -> Business service existente
  -> DTO de negocio
  -> Mapper público
  -> Response externo
```

### Reglas

- No modificar DTOs internos solo para satisfacer un contrato externo.
- Crear DTOs específicos en `Models/Requests/Public` o `Models/Responses/Public`.
- Reutilizar services existentes.
- No duplicar lógica de reservas para Booking.
- No exponer IDs internos.
- No permitir que el cliente público envíe campos calculados críticos como totales finales confiables.
- Validar GUID vacío.
- Validar ownership cuando el endpoint requiera identidad del cliente.

### Adaptación de endpoints existentes

Si existe un endpoint interno y se necesita uno público:

1. Crear request/response público.
2. Mapear GUIDs a IDs mediante services.
3. Llamar al mismo service de negocio.
4. Mapear respuesta ocultando IDs.
5. Agregar validaciones públicas específicas.
6. Agregar tests o pruebas manuales de contrato.

## 19. Convenciones del proyecto

### Namespaces

Mantener estructura:

```text
Servicio.Hotel.{Layer}.{Dominio}.{Tipo}
```

Ejemplos:

- `Servicio.Hotel.Business.Services.Reservas`
- `Servicio.Hotel.DataManagement.Alojamiento.Services`
- `Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion`
- `Servicio.Hotel.API.Controllers.V1.Internal.Reservas`

### Dominios

Usar dominios existentes:

- `Alojamiento`
- `Reservas`
- `Hospedaje`
- `Facturacion`
- `Seguridad`
- `Valoraciones`
- `Booking` para API pública/marketplace.

### Identificadores

Regla:

- Interno/backoffice: IDs numéricos (`IdReserva`, `IdCliente`, `IdHabitacion`).
- Público/marketplace: GUIDs (`ReservaGuid`, `ClienteGuid`, `HabitacionGuid`).

No mezclar ambos en contratos públicos.

### Fechas

Reglas:

- Campos de auditoría y eventos de sistema usan UTC (`FechaRegistroUtc`, `FechaReservaUtc`, `FechaPagoUtc`, etc.).
- Rangos de estadía/reserva usan fechas de negocio (`FechaInicio`, `FechaFin`) y suelen normalizarse con `.Date`.
- No convertir fechas de reserva a local time dentro de controllers sin una política explícita.

### Estados

Mantener códigos existentes:

- Habitaciones: `DIS`, `OCU`, `MNT`, `FDS`, `INA`.
- Reservas: `PEN`, `CON`, `CAN`, `EXP`, `FIN`, `EMI`.
- Usuarios/Roles/Sucursales/Tarifas: típicamente `ACT` para activo.
- Valoraciones: revisar validators/configurations antes de agregar nuevos estados.

No inventar estados nuevos sin actualizar:

- Validator.
- Constraints de base de datos.
- Mappers.
- Documentación.
- Frontend/consumidores si aplica.

## 20. Manejo de errores y responses

### Middleware global

`ExceptionHandlingMiddleware` centraliza excepciones y retorna `ApiErrorResponse`.

Mapeo actual:

- `ValidationException` -> 400.
- `UnauthorizedBusinessException` -> 401.
- `NotFoundException` -> 404.
- `ConflictException` -> 409.
- `BusinessException` -> 422.
- `DomainException` -> 422.
- `DbUpdateException` -> 400/409/422 según constraint.
- `InvalidOperationException` -> 409.
- Excepciones no controladas -> 500.

Regla:

- No envolver cada controller con try/catch si el middleware ya lo maneja.
- Lanzar excepciones de negocio específicas.
- Incluir códigos de error cuando existan patrones (`RES-001`, `AUTH-001`, etc.).

### ApiResponse

Existe `ApiResponse<T>` y `ApiResponse`, pero muchos endpoints actuales retornan directamente `Ok(result)`, `CreatedAtAction(...)` o `NoContent()`.

Recomendación:

- Definir una política única para responses antes de migrar todos los endpoints.
- No mezclar estilos dentro de un mismo nuevo módulo.
- Para errores, conservar `ApiErrorResponse` vía middleware.

## 21. Registro de dependencias

La composición de servicios ocurre en:

```text
Servicio.Hotel.API/Extensions/ServiceCollectionExtensions.cs
```

Patrón:

```csharp
services.AddScoped<IReservaRepository, ReservaRepository>();
services.AddScoped<IReservaDataService, ReservaDataService>();
services.AddScoped<IReservaService, ReservaService>();
```

Reglas:

- Cada nuevo dominio debe registrar repository, data service y business service.
- Registrar interfaces, no implementaciones directas en consumers.
- Para HTTP externo usar `AddHttpClient`.
- Para settings usar `IOptions<T>` y `BindConfiguration`.

## 22. Unit of Work y transacciones

`IUnitOfWork` y `UnitOfWork` encapsulan:

- `SaveChangesAsync`.
- `BeginTransactionAsync`.
- `ExecuteInTransactionAsync`.
- Transacción EF Core actual.

Reglas:

- Usar `ExecuteInTransactionAsync` para operaciones multi-step.
- Si ya existe una transacción, `ExecuteInTransactionAsync` ejecuta la acción dentro de ella.
- No anidar transacciones manualmente.
- No llamar `SaveChangesAsync` desde controllers.
- Data services/repositories deben coordinar guardado según patrón existente.

## 23. Estructura de carpetas recomendada enterprise

Para nuevos módulos, seguir este patrón:

```text
Servicio.Hotel.API
├── Controllers
│   └── V1
│       ├── Internal
│       │   └── NuevoDominio
│       │       └── NuevoRecursoController.cs
│       └── Booking
│           └── NuevoRecursoPublicController.cs
├── Models
│   ├── Requests
│   │   ├── Internal
│   │   │   └── NuevoRecursoRequests.cs
│   │   └── Public
│   │       └── NuevoRecursoPublicRequests.cs
│   └── Responses
│       ├── Internal
│       │   └── NuevoRecursoResponses.cs
│       └── Public
│           └── NuevoRecursoPublicDtos.cs

Servicio.Hotel.Business
├── DTOs
│   └── NuevoDominio
│       ├── NuevoRecursoDTO.cs
│       ├── NuevoRecursoCreateDTO.cs
│       ├── NuevoRecursoUpdateDTO.cs
│       └── NuevoRecursoFiltroDTO.cs
├── Interfaces
│   └── NuevoDominio
│       └── INuevoRecursoService.cs
├── Mappers
│   └── NuevoDominio
│       └── NuevoRecursoBusinessMapper.cs
├── Services
│   └── NuevoDominio
│       └── NuevoRecursoService.cs
└── Validators
    └── NuevoDominio
        └── NuevoRecursoValidator.cs

Servicio.Hotel.DataManagement
├── NuevoDominio
│   ├── Interfaces
│   │   └── INuevoRecursoDataService.cs
│   ├── Mappers
│   │   └── NuevoRecursoDataMapper.cs
│   ├── Models
│   │   └── NuevoRecursoDataModel.cs
│   └── Services
│       └── NuevoRecursoDataService.cs

Servicio.Hotel.DataAccess
├── Entities
│   └── NuevoDominio
│       └── NuevoRecursoEntity.cs
├── Configurations
│   └── NuevoDominio
│       └── NuevoRecursoConfiguration.cs
├── Repositories
│   ├── Interfaces
│   │   └── NuevoDominio
│   │       └── INuevoRecursoRepository.cs
│   └── NuevoDominio
│       └── NuevoRecursoRepository.cs
└── Queries
    └── NuevoDominio
        └── NuevoRecursoQueryRepository.cs
```

## 24. Estrategia futura de microservicios

El sistema está preparado conceptualmente para evolucionar hacia microservicios por bounded contexts.

Separación futura recomendada:

- `hotel-identity-service`: usuarios, roles, JWT, auth centralizado.
- `hotel-accommodation-service`: sucursales, habitaciones, tipos, tarifas, catálogo.
- `hotel-reservation-service`: clientes, reservas, disponibilidad.
- `hotel-stay-service`: estadías, check-in, check-out, cargos.
- `hotel-billing-service`: facturas, pagos, pasarelas.
- `hotel-review-service`: valoraciones y moderación.
- `hotel-public-api` o API Gateway/BFF: contratos públicos tipo Booking.

### API Gateway

Responsabilidades futuras:

- Enrutamiento público e interno.
- Rate limiting.
- Validación JWT centralizada.
- Correlation ID.
- Normalización de errores.
- Versionado de API pública.
- Protección de endpoints administrativos.

### Comunicación interna

Opciones:

- HTTP REST interno para operaciones simples.
- gRPC para comunicación interna de baja latencia y contratos fuertemente tipados.
- Eventos para procesos asincrónicos.

Eventos futuros útiles:

- `ReservaCreada`.
- `ReservaConfirmada`.
- `ReservaCancelada`.
- `FacturaGenerada`.
- `PagoRegistrado`.
- `CheckInRealizado`.
- `CheckOutRealizado`.
- `ValoracionPublicada`.

### Auth centralizado

Cuando exista microservicio de identidad:

- Los demás servicios solo validan tokens.
- No duplicar tabla de usuarios salvo proyecciones read-only.
- Usar scopes/audiences por servicio.
- Usar service tokens para operaciones internas.

## 25. Buenas prácticas para IA/Codex

Antes de programar:

1. Leer este documento.
2. Inspeccionar el módulo existente equivalente.
3. Identificar controller, service, data service, repository, mapper, DTO y validator relacionados.
4. Revisar si ya existe una regla de negocio reutilizable.
5. Revisar rutas actuales y contratos internos/públicos.
6. Revisar si el endpoint debe usar ID o GUID.
7. Revisar si requiere `[Authorize]`, policy o ownership validation.
8. Revisar si requiere transacción.

Al implementar:

- Seguir el flujo `Controller -> Business -> DataManagement -> DataAccess`.
- Crear contratos HTTP en API, no en Business.
- Crear DTOs de negocio en Business.
- Crear DataModels en DataManagement.
- Crear Entities y Configurations en DataAccess.
- Agregar mappers explícitos.
- Registrar dependencias en `ServiceCollectionExtensions`.
- Usar excepciones de negocio existentes.
- Reutilizar validators existentes o agregar uno por dominio.
- Mantener rutas versionadas.
- Mantener nombres en español para dominios internos.
- Mantener GUIDs en API pública.

Evitar:

- Resolver todo dentro del controller.
- Copiar lógica de `ReservaService` a un controller público.
- Usar `GetAllAsync()` y filtrar en memoria para endpoints de alto volumen.
- Exponer `IdCliente`, `IdReserva`, `IdHabitacion` en contratos públicos.
- Agregar campos calculados confiando en valores enviados por el cliente.
- Saltarse `IUnitOfWork` en operaciones compuestas.
- Crear un nuevo patrón de response sin revisar el actual.
- Cambiar estados sin revisar constraints de base de datos.

Checklist mínimo para una nueva feature:

- Controller agregado o actualizado.
- Request/response contracts correctos.
- Interface de Business actualizada.
- Service de Business implementado.
- Validator actualizado.
- Mapper de Business actualizado.
- Interface de DataManagement actualizada.
- DataService implementado.
- DataModel y mapper actualizados.
- Repository/interface actualizado si aplica.
- Entity/configuration actualizada si aplica.
- DI actualizado.
- Swagger compila.
- Build compila.
- Endpoint probado manualmente.
- Errores pasan por middleware.

## 26. Riesgos técnicos actuales

### Duplicación de lógica

Riesgo:

- Reimplementar reservas, disponibilidad o pricing en controllers públicos.

Mitigación:

- Reutilizar `IReservaService`, `IHabitacionService`, `ITarifaService`, `IClienteService` y `ISucursalService`.

### Acoplamiento entre capas

Riesgo:

- Controllers usando DTOs de Business como requests y responses en exceso.
- API referenciando DataAccess fuera de DI.

Mitigación:

- Seguir separando request/response contracts.
- Mantener referencias directas de DataAccess solo para composición.

### DTO leakage

Riesgo:

- Filtrar IDs internos o campos auditables al marketplace.

Mitigación:

- Usar `PublicDto`.
- Usar GUIDs.
- Usar `PublicRequestGuard`.

### Lógica en controllers

Riesgo:

- Controllers públicos acumulando lógica de traducción, ownership, cálculo o búsqueda.

Mitigación:

- Mantener controllers como adaptadores.
- Mover reglas a services.
- Crear application services si un flujo público se vuelve complejo.

### Queries pesadas

Riesgo:

- `GetAllAsync()` + LINQ en memoria.
- N+1 queries en mappers públicos.

Mitigación:

- Query repositories.
- Proyecciones específicas.
- Paginación obligatoria.
- `AsNoTracking()`.

### Validaciones inconsistentes

Riesgo:

- Validar fechas o estados de manera distinta en API pública e interna.

Mitigación:

- Centralizar reglas en validators y Business.
- API solo valida forma del request y restricciones del contrato.

### Refresh tokens en memoria

Riesgo:

- `RevokedRefreshTokens` está en memoria. En despliegues multi-instancia, la revocación no se comparte entre nodos.

Mitigación futura:

- Persistir refresh tokens y revocaciones en base de datos o cache distribuida.

### Rol CLIENTE quemado

Riesgo:

- `AuthService.GetClienteRoleAsync` usa `CLIENTE_ROLE_ID = 4`.

Mitigación:

- Resolver por nombre de rol (`CLIENTE`) o configuración.

## 27. Recomendaciones futuras

### CQRS

Separar comandos y consultas:

- Commands para crear, confirmar, cancelar, pagar.
- Queries para búsqueda, disponibilidad, detalle público y reportes.

Beneficio:

- Evita cargar aggregates completos para consultas públicas.
- Mejora performance y claridad.

### Caching

Candidatos:

- Sucursales activas.
- Tipos de habitación.
- Catálogos de servicios.
- Tarifas vigentes por rango/canal.
- Reviews públicas moderadas.

Reglas:

- No cachear disponibilidad sin invalidación clara.
- Invalidar cache cuando cambian habitaciones, tarifas o reservas.

### Event-driven

Usar eventos para:

- Generar factura tras confirmar reserva.
- Enviar email de confirmación.
- Notificar pago aprobado.
- Solicitar review tras checkout.
- Actualizar proyecciones públicas.

### Observabilidad

Agregar:

- Structured logging.
- Correlation ID.
- Request ID.
- Métricas por endpoint.
- Métricas de disponibilidad y reservas.
- Trazas distribuidas cuando haya microservicios.

### Health checks

Agregar endpoints:

- `/health/live`
- `/health/ready`

Validar:

- SQL Server.
- Pasarela de pagos.
- Servicios externos críticos.

### API Gateway

Recomendado para:

- Rate limiting.
- Protección de API pública.
- Enrutamiento a microservicios.
- Validación de JWT.
- Versionado externo.
- Respuestas uniformes.

### Autenticación distribuida

Futuro:

- Identity service central.
- Scopes por operación.
- Audiences por microservicio.
- Refresh tokens persistidos.
- Revocación distribuida.
- Rotación de secretos/llaves.

## 28. Guía rápida por tipo de cambio

### Agregar endpoint interno

1. Crear o actualizar controller en `Controllers/V1/Internal`.
2. Crear request en `Models/Requests/Internal`.
3. Mapear request a DTO de Business.
4. Llamar service existente o crear método en interface/service.
5. Agregar validación en Business.
6. Agregar DataManagement/Repository solo si falta persistencia.
7. Registrar dependencias si son nuevas.

### Agregar endpoint público

1. Crear request/response en `Models/Requests/Public` y `Models/Responses/Public`.
2. Usar GUIDs.
3. Rechazar IDs numéricos.
4. Resolver GUIDs a IDs con services.
5. Llamar Business.
6. Mapear respuesta ocultando IDs.
7. Agregar ownership validation si requiere usuario autenticado.

### Agregar nueva entidad

1. Crear Entity en DataAccess.
2. Crear Configuration.
3. Agregar `DbSet`.
4. Crear Repository e interface.
5. Crear DataModel.
6. Crear DataMapper.
7. Crear DataService e interface.
8. Crear DTOs Business.
9. Crear BusinessMapper.
10. Crear Service e interface.
11. Crear Validator.
12. Crear Request/Response API.
13. Registrar DI.
14. Crear migration si el proyecto usa migrations.

### Cambiar regla de reserva

1. Revisar `ReservaService`.
2. Revisar `ReservaValidator`.
3. Revisar `ReservaDataService`.
4. Revisar constraints/checks en DataAccess configurations o base de datos.
5. Revisar contratos públicos que dependen del flujo.
6. Probar creación, confirmación, cancelación y cálculo de precio.

## 29. Principio final

La API debe evolucionar como un sistema enterprise modular: cada capa tiene una razón de existir, cada contrato tiene un público distinto y cada regla de negocio debe tener un único lugar confiable.

Cuando haya duda, preferir:

```text
Reutilizar lógica existente
  > crear lógica duplicada

Agregar contrato específico
  > exponer modelo interno

Validar en Business
  > confiar en el controller

Consultar con repository/query repository
  > consultar DbContext desde API

GUID público
  > ID interno en marketplace

Transacción con UnitOfWork
  > operaciones parciales
```

Este documento debe mantenerse actualizado cada vez que cambie la arquitectura, se agregue un microservicio, se modifique el flujo de reservas, se cambie seguridad/JWT o se introduzca una nueva convención transversal.
