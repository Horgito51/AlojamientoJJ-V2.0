# Propuesta tecnica de migracion a microservicios - Servicio.Hotel

## Contexto real analizado

El sistema actual `Servicio.Hotel` es una API ASP.NET Core organizada como monolito modular por capas:

```text
Servicio.Hotel.API
  -> Servicio.Hotel.Business
  -> Servicio.Hotel.DataManagement
  -> Servicio.Hotel.DataAccess
  -> SQL Server HotelJJ
```

La solucion ya separa modulos de negocio en carpetas: `Alojamiento`, `Reservas`, `Hospedaje`, `Facturacion`, `Seguridad`, `Valoraciones` y `Booking`. La base principal usa los esquemas `booking` y `seguridad`. El script tambien contiene esquemas historicos o ajenos al dominio hotelero (`aero`, `vuelos`, `ventas`, `crm` y `seg`); para esta migracion se consideran vigentes las tablas consumidas por EF Core y los controladores actuales: `booking.*` y `seguridad.*`. La tabla `crm.CLIENTES` y las tablas `seg.*` pueden tratarse como legado o fuente historica, no como modelo operativo principal.

La API actual ya distingue dos bordes:

| Borde | Rutas actuales | Uso | Identificadores |
|---|---|---|---|
| Publico / booking | `/api/v1/accommodations`, `/api/v1/public/*`, `/api/v1/auth/*` | Clientes, portal, integraciones externas | GUIDs |
| Interno / backoffice | `/api/v{version}/internal/*` | Administracion, recepcion, operacion hotelera | IDs internos y GUIDs |

La propuesta mantiene esa separacion, pero mueve el acceso publico a un API Gateway central. Los microservicios no se expondran directamente.

---

## 1. Objetivo general de la migracion

El objetivo es evolucionar el monolito modular actual hacia una arquitectura de microservicios con separacion real de dominios, bases de datos por servicio, autenticacion centralizada y un API Gateway como unico punto publico de entrada.

Problemas actuales del monolito:

| Problema | Evidencia en el proyecto actual | Impacto |
|---|---|---|
| Acoplamiento de dominios en una sola BD | `booking` contiene sucursales, habitaciones, reservas, estadias, facturas, pagos y valoraciones; `seguridad.USUARIO_APP` referencia `booking.CLIENTES` | Cambiar un flujo afecta varios modulos y obliga a transacciones compartidas |
| Dependencias cruzadas en Business | `ReservaService` depende de tarifas, habitaciones, tipos de habitacion, facturacion, UnitOfWork y DbContext | Reservas no puede evolucionar o desplegarse sin alojamiento y facturacion |
| Queries agregadas sobre varios dominios | `BookingAccommodationService` consulta sucursales, habitaciones, tipos, tarifas, reservas y valoraciones | El portal publico depende de un modelo relacional compartido |
| Transacciones locales multi-dominio | Confirmar reserva genera factura en la misma transaccion | En microservicios se requiere saga o consistencia eventual |
| Exposicion mixta de APIs | Auth tiene rutas internas y publicas en el mismo controller; pagos tiene ruta interna y una ruta publica absoluta | La politica de seguridad queda distribuida |
| Repositorio unico de persistencia | Un solo `ServicioHotelDbContext` administra todos los agregados | No hay autonomia de datos ni escalabilidad independiente |

Beneficios esperados:

- Escalabilidad independiente: reservas, busqueda de alojamientos y autenticacion pueden escalar diferente.
- Despliegues independientes: cambios en valoraciones o catalogos no deben obligar a desplegar pagos o auth.
- Separacion de dominios DDD: cada bounded context controla sus reglas y datos.
- Seguridad centralizada: Auth emite JWT; Gateway valida, enruta y aplica politicas.
- Menor blast radius: fallas en pagos no deben tumbar busqueda publica de accommodations.
- Preparacion para gRPC interno: operaciones de disponibilidad, pricing, identidad y validacion pueden tener contratos binarios de baja latencia.
- Evolucion progresiva: se puede aplicar strangler pattern manteniendo temporalmente el monolito detras del Gateway.

Arquitectura objetivo de alto nivel:

```text
Cliente Web / Mobile / Integraciones
        |
        v
Gateway.Api
  - JWT validation
  - routing
  - rate limiting
  - correlation id
  - aggregation
  - REST publico
  - gRPC interno opcional
        |
        +--> Auth Service
        +--> Booking/Public Facade Service
        +--> Clientes Service
        +--> Alojamiento Service
        +--> Reservas Service
        +--> Estadias Service
        +--> Facturacion Service
        +--> Pagos Service
        +--> Valoraciones Service
        +--> Auditoria Service
        +--> Notificaciones Service
        +--> Archivos Service
```

---

## 2. Analisis de dominios del negocio

| Dominio | Responsabilidad | Entidades actuales | Logica de negocio actual | Dependencias |
|---|---|---|---|---|
| Seguridad / Auth | Login, registro, refresh, cambio de password, usuarios, roles, autorizacion | `seguridad.USUARIO_APP`, `seguridad.ROL`, `seguridad.USUARIOS_ROLES` | Generacion JWT, validacion credenciales, roles, usuario activo, asociacion usuario-cliente | Clientes para registro de cliente |
| Clientes | Datos del huesped/cliente y busquedas por correo/identificacion | `booking.CLIENTES` | Crear cliente, actualizar, inhabilitar, buscar por email o identificacion | Auth lo referencia por `cliente_guid`; reservas, facturacion y valoraciones lo consumen |
| Alojamiento / Inventario | Sucursales, habitaciones, tipos, catalogo de servicios, imagenes | `booking.SUCURSAL`, `HABITACION`, `TIPO_HABITACION`, `CATALOGO_SERVICIOS`, `SUCURSAL_IMAGENES`, `TIPO_HABITACION_IMAGEN`, `TIPO_HABITACION_CATALOGO` | Estado reservable, politicas, amenities, capacidad, publicacion en portal | Reservas, estadias, booking publico, valoraciones |
| Tarifas / Pricing | Tarifas por sucursal, tipo, canal y rango | `booking.TARIFA` | Tarifa vigente por rango, prioridad, min/max noches, portal publico, IVA | Reservas y booking publico |
| Reservas | Cabecera y detalle de reserva por habitaciones | `booking.RESERVAS`, `booking.RESERVAS_HABITACIONES` | Crear, calcular precio, confirmar, cancelar, validar solapamientos, reservar por tipo | Clientes, alojamiento, tarifas, facturacion, pagos |
| Estadias / Hospedaje | Check-in, checkout, cargos durante estadia | `booking.ESTADIAS`, `booking.CARGO_ESTADIA` | Crear estadia desde reserva, registrar checkout, cargos, anulacion de cargos | Reservas, habitaciones, catalogo, facturacion |
| Facturacion | Facturas y detalles contables | `booking.FACTURAS`, `booking.FACTURA_DETALLE` | Generar factura de reserva, generar factura final, anular, saldo pendiente | Clientes, reservas, estadias/cargos, pagos |
| Pagos | Registro y simulacion/procesamiento de pagos | `booking.PAGOS` | Pago simulado, estado pago, proveedor pasarela, saldo factura | Facturacion, reservas, pasarela externa |
| Valoraciones | Reviews de estadia y moderacion | `booking.VALORACIONES` | Crear review una vez por estadia, moderar, responder, publicar en portal | Estadias, clientes, sucursales, habitaciones |
| Booking/Public | Flujo publico tipo marketplace | DTOs `BookingAccommodationDTOs`, `BookingReservationDTOs`, controllers publicos | Search accommodations, detalle, reviews, habitaciones disponibles, reserva publica con GUIDs | Alojamiento, tarifas, reservas, clientes, valoraciones |
| Auditoria | Trazabilidad de operaciones | `seguridad.AUDITORIA` | Consultas de auditoria, registro de cambios | Todos publican eventos/auditoria |
| Notificaciones | Email/SMS/eventos de reserva, pago, factura | No existe como tabla principal actual | Nuevo dominio recomendado | Reservas, pagos, facturacion, auth |
| Archivos / Imagenes | Imagenes de sucursales y tipos | `SUCURSAL_IMAGENES`, `TIPO_HABITACION_IMAGEN`, urls en tablas | Gestion de URLs, imagen principal, orden | Alojamiento y booking publico |

---

## 3. Propuesta de separacion en microservicios

### 3.1 Microservicio.Auth

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_auth` |
| Esquema | `auth` |
| Tablas propias | `usuarios`, `roles`, `usuarios_roles`, `refresh_tokens`, `password_history` |
| Tablas migradas | `seguridad.USUARIO_APP`, `seguridad.ROL`, `seguridad.USUARIOS_ROLES` |
| Responsabilidad | Autenticacion, autorizacion, emision de JWT, refresh tokens, administracion de usuarios y roles |
| APIs publicas por Gateway | `POST /api/v1/auth/login`, `POST /api/v1/auth/register-cliente`, `POST /api/v1/auth/refresh`, `POST /api/v1/auth/logout`, `POST /api/v1/auth/cambiar-password` |
| APIs internas | `GET /internal/auth/users/{usuarioGuid}`, `POST /internal/auth/token/introspect`, `GET /internal/auth/roles/{rolGuid}` |
| Integracion | Publica `UsuarioCreado`, `UsuarioInhabilitado`, `RolAsignado` |
| Dependencias | Clientes para asociar `cliente_guid` en registro de cliente |

El JWT debe incluir `sub=usuario_guid`, `username`, `email`, `roles`, `cliente_guid` cuando aplique, `jti`, `iat`, `exp`, `iss`, `aud`. Los microservicios no deben generar JWT. Validan firma localmente con configuracion compartida o confian en claims propagados por Gateway mas validacion defensiva.

### 3.2 Microservicio.Clientes

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_clientes` |
| Esquema | `cli` |
| Tablas propias | `clientes`, `cliente_contactos`, `cliente_identidades` opcional |
| Tablas migradas | `booking.CLIENTES`; `crm.CLIENTES` solo si se decide absorber legado |
| Responsabilidad | Perfil de cliente, datos fiscales/contacto, busqueda por email o identificacion |
| APIs publicas por Gateway | `GET /api/v1/public/clientes/{clienteGuid}`, `GET /api/v1/public/clientes/by-email`, `POST /api/v1/public/clientes` |
| APIs internas | `GET /internal/clientes`, `GET /internal/clientes/{id|guid}`, `PUT /internal/clientes/{guid}`, `DELETE /internal/clientes/{guid}` |
| Integracion | Publica `ClienteCreado`, `ClienteActualizado`, `ClienteInhabilitado` |
| Dependencias | Ninguna fuerte; Auth consume por GUID |

Debe eliminarse la FK fisica desde Auth. Auth guarda `cliente_guid` como referencia externa.

### 3.3 Microservicio.Alojamiento

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_alojamiento` |
| Esquema | `alo` |
| Tablas propias | `sucursales`, `habitaciones`, `tipos_habitacion`, `catalogo_servicios`, `sucursal_imagenes`, `tipo_habitacion_imagenes`, `tipo_habitacion_catalogo` |
| Tablas migradas | `booking.SUCURSAL`, `HABITACION`, `TIPO_HABITACION`, `CATALOGO_SERVICIOS`, `SUCURSAL_IMAGENES`, `TIPO_HABITACION_IMAGEN`, `TIPO_HABITACION_CATALOGO` |
| Responsabilidad | Inventario hotelero, estados de habitaciones, politicas de sucursal, amenities |
| APIs publicas por Gateway | `GET /api/v1/public/sucursales/{sucursalGuid}`, `GET /api/v1/public/habitaciones`, `GET /api/v1/public/habitaciones/{habitacionGuid}`, `GET /api/v1/public/tipos-habitacion/{tipoHabitacionGuid}` |
| APIs internas | CRUD de `/internal/sucursales`, `/internal/habitaciones`, `/internal/tipos-habitacion`, `/internal/catalogo-servicios` |
| gRPC recomendado | `GetHabitacion`, `GetSucursal`, `GetTipoHabitacion`, `GetHabitacionesDisponiblesPorSucursal`, `ValidateHabitacionesReservables` |
| Dependencias | Tarifas para precio publicado si se mantiene separado; Reservas consume alojamiento |

El microservicio controla estados fisicos: `DIS`, `OCU`, `MNT`, `INA`, etc. Reservas no debe modificar directamente habitaciones; debe solicitar cambios por comando interno o reaccionar a eventos.

### 3.4 Microservicio.Tarifas

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_tarifas` |
| Esquema | `tar` |
| Tablas propias | `tarifas`, `tarifa_reglas`, `tarifa_historial` |
| Tablas migradas | `booking.TARIFA` |
| Responsabilidad | Pricing por sucursal, tipo, canal, vigencia, prioridad, IVA y restricciones |
| APIs publicas por Gateway | Lecturas integradas dentro de accommodations, no exponer CRUD publico |
| APIs internas | `GET /internal/tarifas`, `POST /internal/tarifas`, `PUT /internal/tarifas/{tarifaGuid}`, `PATCH /internal/tarifas/{tarifaGuid}/desactivar` |
| gRPC recomendado | `GetTarifaVigente`, `CalculateRoomPrice`, `GetTarifasActivasPortal` |
| Dependencias | Alojamiento por `sucursal_guid` y `tipo_habitacion_guid` |

Las FKs `id_sucursal` e `id_tipo_habitacion` pasan a referencias externas `sucursal_guid` y `tipo_habitacion_guid`.

### 3.5 Microservicio.Reservas

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_reservas` |
| Esquema | `rsv` |
| Tablas propias | `reservas`, `reservas_habitaciones`, `reserva_estado_historial`, `reserva_outbox` |
| Tablas migradas | `booking.RESERVAS`, `booking.RESERVAS_HABITACIONES` |
| Responsabilidad | Crear, calcular, confirmar, cancelar y consultar reservas; evitar solapamientos |
| APIs publicas por Gateway | `GET /api/v1/public/reservas/{reservaGuid}`, `GET /api/v1/public/reservas`, `POST /api/v1/public/reservas`, `PATCH /api/v1/public/reservas/{reservaGuid}/cancelar`, `POST /api/v1/public/reservas/calcular-precio` |
| APIs internas | `GET/POST/PUT /internal/reservas`, `PATCH /internal/reservas/{id|guid}/confirmar`, `PATCH /internal/reservas/{id|guid}/cancelar`, `POST /internal/reservas/calcular-precio` |
| gRPC recomendado | `CheckAvailability`, `HoldRooms`, `ConfirmReservation`, `GetReservaSnapshot` |
| Dependencias | Clientes, Alojamiento, Tarifas, Facturacion, Pagos |

La regla actual de `ReservaService` debe preservarse: una reserva requiere habitaciones, fechas validas, estado reservable, no solapamiento con reservas `PEN` o `CON`, calculo de precio por tarifa vigente o precio base y total con IVA. La confirmacion ya no debe generar factura en transaccion local; debe publicar `ReservaConfirmada`.

### 3.6 Microservicio.Estadias

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_estadias` |
| Esquema | `est` |
| Tablas propias | `estadias`, `cargos_estadia`, `estadia_estado_historial` |
| Tablas migradas | `booking.ESTADIAS`, `booking.CARGO_ESTADIA` |
| Responsabilidad | Check-in, checkout, consumos/cargos, cierre operativo de estadia |
| APIs internas | `GET /internal/estadias`, `POST /internal/estadias/checkin/{reservaGuid}`, `PATCH /internal/estadias/{estadiaGuid}/checkout`, `GET/POST /internal/estadias/{estadiaGuid}/cargos`, `PATCH /internal/cargos-estadia/{cargoGuid}/anular` |
| APIs publicas | No exponer directamente, salvo consulta resumida de estadia del cliente via Gateway si se requiere |
| gRPC recomendado | `GetEstadia`, `GetCargosByReserva`, `HasCompletedStay` |
| Dependencias | Reservas, Alojamiento, Facturacion |

Las FKs actuales `id_reserva_habitacion`, `id_cliente`, `id_habitacion`, `id_catalogo` pasan a GUIDs externos: `reserva_habitacion_guid`, `cliente_guid`, `habitacion_guid`, `catalogo_guid`.

### 3.7 Microservicio.Facturacion

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_facturacion` |
| Esquema | `fac` |
| Tablas propias | `facturas`, `factura_detalle`, `factura_estado_historial`, `invoice_outbox` |
| Tablas migradas | `booking.FACTURAS`, `booking.FACTURA_DETALLE` |
| Responsabilidad | Emision, anulacion, saldo, detalle fiscal de reserva y estadia |
| APIs internas | `GET /internal/facturas`, `GET /internal/facturas/{facturaGuid}`, `POST /internal/facturas/generar-reserva/{reservaGuid}`, `POST /internal/facturas/generar-final/{reservaGuid}`, `PATCH /internal/facturas/{facturaGuid}/anular` |
| APIs publicas | `GET /api/v1/public/facturas/{facturaGuid}` si el cliente necesita consultar documentos |
| gRPC recomendado | `GetFactura`, `GenerateReservationInvoice`, `ApplyPayment`, `GetSaldoPendiente` |
| Dependencias | Reservas, Clientes, Estadias, Pagos |

`referencia_id` en `FACTURA_DETALLE` debe cambiar a `referencia_guid`, manteniendo `referencia_tipo` para apuntar a `RESERVA_HABITACION`, `CARGO_ESTADIA`, `SERVICIO`, etc.

### 3.8 Microservicio.Pagos

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_pagos` |
| Esquema | `pag` |
| Tablas propias | `pagos`, `pago_intentos`, `pago_webhooks`, `pago_estado_historial` |
| Tablas migradas | `booking.PAGOS` |
| Responsabilidad | Procesar/simular pagos, registrar transaccion externa, estados y conciliacion |
| APIs publicas por Gateway | `POST /api/v1/public/pagos/simular`, futuro `POST /api/v1/public/pagos` |
| APIs internas | `GET /internal/pagos`, `GET /internal/pagos/factura/{facturaGuid}`, `PATCH /internal/pagos/{pagoGuid}/estado` |
| gRPC recomendado | `RegisterPayment`, `GetTotalPagadoPorFactura` |
| Dependencias | Facturacion, Reservas, pasarela externa |

El actual `HttpPaymentGateway` debe moverse a infraestructura de este servicio.

### 3.9 Microservicio.Valoraciones

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_valoraciones` |
| Esquema | `val` |
| Tablas propias | `valoraciones`, `valoracion_moderacion_historial` |
| Tablas migradas | `booking.VALORACIONES` |
| Responsabilidad | Reviews, puntuaciones, moderacion, respuesta del hotel y publicacion |
| APIs publicas por Gateway | `GET /api/v1/accommodations/{sucursalGuid}/reviews`, futuro `POST /api/v1/public/valoraciones` |
| APIs internas | `GET/POST /internal/valoraciones`, `PATCH /internal/valoraciones/{valoracionGuid}/moderar`, `PATCH /internal/valoraciones/{valoracionGuid}/responder`, `DELETE /internal/valoraciones/{valoracionGuid}` |
| gRPC recomendado | `GetPublishedReviewsBySucursal`, `GetRatingSummary` |
| Dependencias | Estadias, Clientes, Alojamiento |

La regla actual de una valoracion por estadia debe preservarse usando `estadia_guid` con indice unico.

### 3.10 Microservicio.BookingPublic

| Aspecto | Propuesta |
|---|---|
| Base de datos | Opcional: `hotel_booking_read` |
| Esquema | `book` |
| Tablas propias | `accommodation_search_read_model`, `availability_cache`, `rating_summary_cache` opcionales |
| Responsabilidad | Fachada publica de busqueda/detalle/reserva tipo marketplace |
| APIs publicas por Gateway | `GET /api/v1/accommodations/search`, `GET /api/v1/accommodations/{sucursalGuid}`, `GET /api/v1/accommodations/{sucursalGuid}/reviews`, `GET /api/v1/accommodations/sucursales/{sucursalGuid}/habitaciones`, `POST /api/v1/accommodations/reservas`, `GET /api/v1/accommodations/reservas/{reservaGuid}` |
| APIs internas | Rebuild de read models, health interno |
| Integracion | Consume eventos de Alojamiento, Tarifas, Reservas, Valoraciones |
| Dependencias | Alojamiento, Tarifas, Reservas, Valoraciones, Clientes |

Este servicio puede empezar como orquestador sin BD propia y luego evolucionar a read model denormalizado para busqueda. La logica actual de `BookingAccommodationService` es el candidato natural para extraerse.

### 3.11 Microservicio.Auditoria

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_auditoria` |
| Esquema | `aud` |
| Tablas propias | `auditoria`, `integration_logs`, `security_logs` |
| Tablas migradas | `seguridad.AUDITORIA`, opcional `crm.AUDITORIA_LOG` |
| Responsabilidad | Trazabilidad transversal, consulta administrativa, retencion |
| APIs internas | `GET /internal/auditoria`, `GET /internal/auditoria/{auditoriaGuid}` |
| Integracion | Consume eventos de todos los servicios |
| Dependencias | Ninguna fuerte |

### 3.12 Microservicio.Archivos

| Aspecto | Propuesta |
|---|---|
| Base de datos | `hotel_archivos` |
| Esquema | `arc` |
| Tablas propias | `archivos`, `imagenes_sucursal`, `imagenes_tipo_habitacion` o catalogo generico |
| Tablas divididas | Puede absorber `SUCURSAL_IMAGENES` y `TIPO_HABITACION_IMAGEN` si se separa de Alojamiento |
| Responsabilidad | URLs, metadata, orden, imagen principal, gestion de assets |
| APIs internas | `POST /internal/archivos`, `GET /internal/imagenes/sucursal/{sucursalGuid}` |
| Dependencias | Alojamiento |

Para una primera fase, las imagenes pueden quedarse dentro de Alojamiento. Separarlas tiene sentido si crece el manejo de archivos.

---

## 4. Diseno de bases de datos por microservicio

| Base destino | Esquema | Tablas migradas | Cambios clave |
|---|---|---|---|
| `hotel_auth` | `auth` | `seguridad.USUARIO_APP`, `ROL`, `USUARIOS_ROLES` | `id_cliente` -> `cliente_guid`; refresh tokens separados |
| `hotel_clientes` | `cli` | `booking.CLIENTES` | Mantener `cliente_guid` global; datos fiscales/contacto |
| `hotel_alojamiento` | `alo` | `SUCURSAL`, `HABITACION`, `TIPO_HABITACION`, `CATALOGO_SERVICIOS`, imagenes, catalogos tipo | FKs internas se mantienen solo dentro del servicio; exponer GUIDs |
| `hotel_tarifas` | `tar` | `TARIFA` | `id_sucursal`, `id_tipo_habitacion` -> GUIDs externos |
| `hotel_reservas` | `rsv` | `RESERVAS`, `RESERVAS_HABITACIONES` | `id_cliente`, `id_sucursal`, `id_habitacion`, `id_tarifa` -> GUIDs externos; snapshot de precio |
| `hotel_estadias` | `est` | `ESTADIAS`, `CARGO_ESTADIA` | Referencias por GUID a reserva, habitacion, cliente, catalogo |
| `hotel_facturacion` | `fac` | `FACTURAS`, `FACTURA_DETALLE` | Referencias por GUID; `referencia_id` -> `referencia_guid` |
| `hotel_pagos` | `pag` | `PAGOS` | `factura_guid`, `reserva_guid`; intentos/webhooks |
| `hotel_valoraciones` | `val` | `VALORACIONES` | `estadia_guid`, `cliente_guid`, `sucursal_guid`, `habitacion_guid` |
| `hotel_auditoria` | `aud` | `seguridad.AUDITORIA` | Recibe eventos de auditoria |
| `hotel_booking_read` | `book` | Nuevo read model opcional | Denormalizacion para busqueda publica |

Tablas que deben dividirse o redisenarse:

| Tabla actual | Problema | Propuesta |
|---|---|---|
| `booking.FACTURA_DETALLE` | Usa `referencia_id` generico local | Usar `referencia_guid` y `referencia_tipo`; almacenar descripcion y montos como snapshot |
| `booking.PAGOS` | Referencia factura y reserva por IDs locales | Separar intentos, webhooks y pagos; usar `factura_guid` y `reserva_guid` |
| `seguridad.USUARIO_APP` | FK a `booking.CLIENTES` | Guardar `cliente_guid` sin FK fisica |
| `booking.RESERVAS_HABITACIONES` | FK a habitacion/tarifa | Guardar `habitacion_guid`, `tarifa_guid`, nombre/tipo/precio snapshot |
| `booking.CARGO_ESTADIA` | FK a catalogo | Guardar `catalogo_guid` opcional y descripcion/precio snapshot |

Tablas que se pueden replicar como read model:

| Datos | Productor | Consumidores | Estrategia |
|---|---|---|---|
| Sucursal publica | Alojamiento | BookingPublic, Reservas, Valoraciones | Eventos `SucursalPublicada`, `SucursalActualizada` |
| Habitacion resumida | Alojamiento | Reservas, BookingPublic, Estadias | gRPC para validacion; cache/read model para busqueda |
| Tarifa vigente | Tarifas | Reservas, BookingPublic | gRPC para calculo; eventos para read model |
| Rating resumen | Valoraciones | BookingPublic, Alojamiento | Evento `ValoracionPublicada` actualiza agregado |
| Cliente minimo | Clientes | Auth, Reservas, Facturacion | Referencia por GUID; snapshots en reservas/facturas |

Integridad entre microservicios:

- No usar FKs fisicas entre bases.
- Cada agregado mantiene su consistencia interna con transacciones locales.
- Las referencias externas se guardan como GUIDs globales.
- Para datos historicos y legales, guardar snapshots: nombre del cliente, identificacion, nombre sucursal, habitacion, tarifa aplicada, descripcion item.
- Usar outbox por servicio para publicar eventos despues de commit local.
- Usar idempotency keys en comandos publicos sensibles: crear reserva, simular/pagar, confirmar reserva.
- Evitar joins distribuidos. Si una pantalla necesita datos compuestos, el Gateway orquesta o BookingPublic mantiene read models.

Estrategia de GUIDs:

- Mantener todos los `*_guid` existentes como identificadores publicos y de integracion.
- Nuevas tablas deben tener `guid` obligatorio, unico y no mutable.
- IDs numericos quedan solo como claves internas dentro de cada base.
- Los contratos REST publicos nunca aceptan IDs numericos.
- Los contratos gRPC internos pueden aceptar GUIDs; IDs solo son validos dentro del proceso propietario.

---

## 5. Relacion entre microservicios

Mapa textual:

```text
Gateway
  -> Auth: login, refresh, introspection opcional
  -> BookingPublic: accommodations y flujo publico
  -> Backoffice services: rutas internal protegidas

BookingPublic
  -> Alojamiento: detalle sucursal, habitaciones, amenities
  -> Tarifas: tarifas publicas y precio desde
  -> Reservas: crear/consultar/cancelar reserva
  -> Valoraciones: reviews publicadas
  -> Clientes: crear o resolver cliente publico

Reservas
  -> Clientes: validar cliente_guid
  -> Alojamiento: validar sucursal/habitaciones reservables
  -> Tarifas: calcular precio vigente
  -> Facturacion: evento ReservaConfirmada
  -> Pagos: consultar pagos asociados si aplica

Estadias
  -> Reservas: obtener reserva confirmada
  -> Alojamiento: marcar habitacion ocupada/mantenimiento
  -> Facturacion: evento CargoEstadiaCreado / CheckoutRealizado

Facturacion
  -> Reservas: snapshot de reserva
  -> Estadias: cargos finales
  -> Clientes: datos fiscales snapshot
  -> Pagos: recibe PagoRegistrado

Pagos
  -> Facturacion: validar factura y aplicar pago
  -> Reservas: asociar pago de reserva

Valoraciones
  -> Estadias: validar estadia finalizada
  -> Clientes/Alojamiento: referencias externas
```

Operaciones sincronas:

| Operacion | Tipo | Motivo |
|---|---|---|
| Gateway -> Auth validar JWT/configuracion | Sincrona/local | Debe ocurrir antes de enrutar |
| Reservas -> Alojamiento validar habitaciones | gRPC sincrono | Necesario antes de crear/confirmar |
| Reservas -> Tarifas calcular precio | gRPC sincrono | Debe responder al usuario en tiempo real |
| Estadias -> Reservas obtener reserva confirmada | gRPC/REST interno | Check-in requiere consistencia inmediata |
| Pagos -> Facturacion validar factura | gRPC sincrono | Evita pagos contra factura inexistente/anulada |

Operaciones asincronas:

| Evento | Productor | Consumidores |
|---|---|---|
| `ReservaCreada` | Reservas | Auditoria, Notificaciones, BookingPublic |
| `ReservaConfirmada` | Reservas | Facturacion, Notificaciones, Alojamiento/Inventario |
| `ReservaCancelada` | Reservas | Facturacion, Pagos, Notificaciones, BookingPublic |
| `FacturaGenerada` | Facturacion | Pagos, Notificaciones, Auditoria |
| `PagoRegistrado` | Pagos | Facturacion, Reservas, Auditoria |
| `CheckinRealizado` | Estadias | Alojamiento, Auditoria |
| `CheckoutRealizado` | Estadias | Facturacion, Alojamiento, Valoraciones |
| `ValoracionPublicada` | Valoraciones | BookingPublic, Alojamiento |
| `SucursalActualizada` | Alojamiento | BookingPublic, Reservas |

---

## 6. Propuesta de APIs REST vs gRPC

### REST publico por Gateway

Debe permanecer REST todo endpoint consumido por frontend, portal, app movil o integracion externa:

| Endpoint actual | Servicio destino | Exposicion |
|---|---|---|
| `POST /api/v1/auth/login` | Auth | Publica |
| `POST /api/v1/auth/register-cliente` | Auth + Clientes | Publica |
| `POST /api/v1/auth/refresh` | Auth | Publica |
| `GET /api/v1/accommodations/search` | BookingPublic | Publica |
| `GET /api/v1/accommodations/{sucursalGuid}` | BookingPublic | Publica |
| `GET /api/v1/accommodations/{sucursalGuid}/reviews` | BookingPublic/Valoraciones | Publica |
| `GET /api/v1/accommodations/sucursales/{sucursalGuid}/habitaciones` | BookingPublic/Alojamiento | Publica |
| `POST /api/v1/accommodations/reservas` | BookingPublic -> Reservas | Publica |
| `GET /api/v1/accommodations/reservas/{reservaGuid}` | BookingPublic -> Reservas | Publica |
| `GET /api/v1/public/reservas/{reservaGuid}` | Reservas | Publica autenticada |
| `POST /api/v1/public/reservas` | Reservas | Publica autenticada o semi-publica segun canal |
| `PATCH /api/v1/public/reservas/{reservaGuid}/cancelar` | Reservas | Publica autenticada |
| `POST /api/v1/public/reservas/calcular-precio` | Reservas | Publica |
| `POST /api/v1/public/pagos/simular` | Pagos | Publica autenticada |

### REST interno por Gateway

Los endpoints `/api/v{version}/internal/*` deben permanecer REST para backoffice y administracion, pero expuestos solamente por Gateway con politicas de rol:

| Grupo actual | Servicio destino |
|---|---|
| `/internal/usuarios`, `/internal/roles`, `/internal/permisos`, `/internal/auth` | Auth |
| `/internal/auditoria` | Auditoria |
| `/internal/sucursales`, `/internal/habitaciones`, `/internal/tipos-habitacion`, `/internal/catalogo-servicios` | Alojamiento |
| `/internal/tarifas` | Tarifas |
| `/internal/clientes` | Clientes |
| `/internal/reservas` | Reservas |
| `/internal/estadias`, `/internal/cargos-estadia` | Estadias |
| `/internal/facturas` | Facturacion |
| `/internal/pagos` | Pagos |
| `/internal/valoraciones` | Valoraciones |

### gRPC interno recomendado

| Contrato gRPC | Servicio propietario | Consumidores | Justificacion |
|---|---|---|---|
| `ValidateToken/IntrospectToken` | Auth | Gateway, servicios internos | Latencia baja, contrato estable |
| `GetHabitacionesDisponibles` | Alojamiento/Reservas segun diseno final | BookingPublic, Reservas | Consulta frecuente, sensible a performance |
| `ValidateHabitacionesReservables` | Alojamiento | Reservas | Evita multiples llamadas REST |
| `GetTarifaVigente` | Tarifas | Reservas, BookingPublic | Pricing debe ser rapido y determinista |
| `CalculateRoomPrice` | Tarifas | Reservas | Sustituye parte de `ReservaService.CalcularPrecioHabitacionAsync` |
| `GetReservaSnapshot` | Reservas | Facturacion, Estadias, Pagos | Evita exponer modelo completo |
| `GenerateReservationInvoice` | Facturacion | Reservas/Saga | Operacion interna no publica |
| `ApplyPayment` | Facturacion | Pagos | Actualiza saldo de forma controlada |
| `GetRatingSummary` | Valoraciones | BookingPublic | Agregado de lectura frecuente |

Razon tecnica:

- REST queda para contratos publicos, integraciones humanas y backoffice.
- gRPC se usa para operaciones internas de alta frecuencia, baja latencia y contratos estrictos.
- No se debe migrar todo a gRPC: CRUD administrativo, busqueda publica y flujos de usuario son mas mantenibles como REST.
- gRPC no reemplaza eventos. Comandos que no requieren respuesta inmediata deben ser asincronos.

---

## 7. Middleware / API Gateway

El API Gateway sera el unico punto de entrada publico. Los microservicios internos no deben publicar rutas accesibles directamente desde clientes externos.

Responsabilidades:

| Componente | Responsabilidad |
|---|---|
| Autenticacion JWT | Validar firma, issuer, audience, expiracion y roles antes de enrutar |
| Autorizacion | Politicas `BackOffice`, `AdminProfile`, `Cliente`, scopes por ruta |
| Routing | Mapear rutas publicas e internas a servicios destino |
| Orquestacion | Componer flujos como registro cliente, accommodations detail o reserva publica |
| Aggregation | Combinar alojamiento, tarifas, rating y disponibilidad sin joins distribuidos |
| Rate limiting | Limites por IP, usuario, cliente y endpoint sensible |
| Logging | Logs estructurados por `correlationId`, usuario, ruta, servicio destino |
| Correlation IDs | Crear o propagar `X-Correlation-Id` en todas las llamadas |
| Errores | Normalizar respuestas tipo `ApiErrorResponse` |
| Transformacion DTO | Convertir contratos publicos a contratos internos cuando sea necesario |
| Seguridad | Ocultar endpoints internos, validar payload publico sin IDs numericos |
| gRPC bridge | Usar clientes gRPC para llamadas internas de alto rendimiento |

Estructura propuesta:

```text
Gateway
  Gateway.Api
    Controllers
    Middleware
    Routing
    Filters
    Program.cs
  Gateway.Application
    UseCases
    Aggregators
    Policies
    DTOs
  Gateway.Infrastructure
    HttpClients
    GrpcClients
    AuthValidation
    Resilience
  Gateway.Shared
    ApiResponse
    Errors
    Correlation
    Contracts
```

Flujo de request:

```text
HTTP request
  -> CorrelationIdMiddleware
  -> ExceptionMiddleware
  -> RateLimitMiddleware
  -> JwtAuthentication
  -> AuthorizationPolicy
  -> RouteResolver
  -> REST/gRPC client
  -> Microservicio
  -> Response transform
  -> ApiResponse
```

Reglas obligatorias:

- El Gateway valida JWT centralizado emitido por Auth.
- Auth es el unico que emite tokens.
- Los microservicios deben validar que la llamada venga del Gateway mediante red interna/logical trust, header firmado interno o client credentials internos.
- Los endpoints internos no se publican fuera del Gateway.
- El Gateway no debe contener reglas profundas de dominio; solo orquestacion y transformacion.

---

## 8. Propuesta de estructura fisica de solucion

Estructura recomendada:

```text
/src
  /Gateway
    /Gateway.Api
    /Gateway.Application
    /Gateway.Infrastructure
    /Gateway.Shared

  /Microservicio.Auth
    /Auth.Api
    /Auth.Application
    /Auth.Domain
    /Auth.Infrastructure
    /Auth.Contracts

  /Microservicio.Clientes
    /Clientes.Api
    /Clientes.Application
    /Clientes.Domain
    /Clientes.Infrastructure
    /Clientes.Contracts

  /Microservicio.Alojamiento
  /Microservicio.Tarifas
  /Microservicio.Reservas
  /Microservicio.Estadias
  /Microservicio.Facturacion
  /Microservicio.Pagos
  /Microservicio.Valoraciones
  /Microservicio.Auditoria
  /Microservicio.BookingPublic
  /Microservicio.Notificaciones

  /BuildingBlocks
    /BuildingBlocks.Domain
    /BuildingBlocks.Application
    /BuildingBlocks.Infrastructure
    /BuildingBlocks.Contracts

  /Contracts
    /Rest
    /Grpc
    /Events
```

Clean Architecture por microservicio:

```text
Api
  -> Application
      -> Domain
  -> Infrastructure
      -> Application abstractions
```

El `Domain` no depende de EF, HTTP, JWT ni SQL. `Application` contiene casos de uso, comandos, queries, validadores y puertos. `Infrastructure` implementa repositories, DbContext, mensajeria, clientes gRPC/HTTP y pasarelas externas.

Shared Kernel:

Debe ser pequeno. Puede contener:

- `Entity`, `ValueObject`, `DomainEvent`.
- `Result`, `PagedResult`.
- Excepciones base.
- `CorrelationContext`.
- Contratos de eventos versionados.
- Utilidades comunes no relacionadas con dominio especifico.

No debe contener:

- DTOs internos de todos los servicios.
- Reglas de reserva, tarifas o facturacion.
- Entidades EF compartidas.
- DbContext compartido.

CQRS:

- Aplicar CQRS fuerte en `BookingPublic` y `Reservas`.
- Comandos: crear reserva, confirmar, cancelar, check-in, checkout, registrar pago.
- Queries: busqueda accommodations, disponibilidad, facturas, historial.
- Read models denormalizados para busqueda publica y dashboard administrativo.

---

## 9. Estrategia de migracion

### Fase 1 - Preparacion y Gateway delante del monolito

Objetivo: no romper el sistema actual.

Acciones:

1. Crear `Gateway` y enrutar todas las rutas actuales hacia el monolito.
2. Centralizar validacion JWT en Gateway usando la misma configuracion actual de `JwtSettings`.
3. Clasificar rutas publicas e internas segun Swagger/controladores actuales.
4. Agregar `X-Correlation-Id` y logging estructurado.
5. Mantener `Servicio.Hotel` intacto detras del Gateway.
6. Definir contratos REST/gRPC y eventos versionados.

Resultado: el cliente solo conoce el Gateway, aunque internamente siga existiendo el monolito.

### Fase 2 - Extraer Auth y Clientes

Motivo: Auth es transversal y requisito de seguridad global.

Acciones:

1. Migrar `seguridad.USUARIO_APP`, `ROL`, `USUARIOS_ROLES` a `hotel_auth`.
2. Migrar `booking.CLIENTES` a `hotel_clientes`.
3. Cambiar `id_cliente` en Auth por `cliente_guid`.
4. Gateway redirige `/auth`, `/internal/usuarios`, `/internal/roles`, `/public/clientes` a nuevos servicios.
5. Mantener una tabla de equivalencias temporal: `old_id`, `guid`, `service`.
6. Monolito consume Auth/Clientes por HTTP/gRPC temporalmente o usa sincronizacion read-only.

Pruebas:

- Login, refresh, registro cliente, rol cliente.
- Validar que las reservas sigan resolviendo cliente.

### Fase 3 - Extraer Alojamiento, Tarifas y BookingPublic

Motivo: flujo publico de accommodations es lectura intensiva y ya esta logicamente separado.

Acciones:

1. Migrar tablas de sucursales, habitaciones, tipos, catalogos e imagenes a `hotel_alojamiento`.
2. Migrar `TARIFA` a `hotel_tarifas`.
3. Extraer `BookingAccommodationService` a `Microservicio.BookingPublic`.
4. Crear read model inicial para accommodations o agregacion via gRPC.
5. Gateway redirige `/api/v1/accommodations/*` y `/public/habitaciones`, `/public/sucursales`, `/public/tipos-habitacion`.
6. Reservas del monolito todavia pueden consultar disponibilidad al nuevo Alojamiento/Tarifas.

Pruebas:

- Search con destino, fechas, precio y capacidad.
- Detalle de sucursal, reviews, habitaciones disponibles.
- Validacion de que no se expongan IDs numericos.

### Fase 4 - Extraer Reservas, Facturacion, Pagos, Estadias y Valoraciones

Motivo: son dominios transaccionales y requieren saga/eventos.

Acciones:

1. Migrar `RESERVAS` y `RESERVAS_HABITACIONES` a `hotel_reservas`.
2. Implementar outbox y eventos `ReservaCreada`, `ReservaConfirmada`, `ReservaCancelada`.
3. Cambiar confirmacion: ya no genera factura en transaccion local; publica evento y Facturacion reacciona.
4. Migrar `FACTURAS`, `FACTURA_DETALLE` y `PAGOS`.
5. Migrar `ESTADIAS`, `CARGO_ESTADIA`.
6. Migrar `VALORACIONES`.
7. Apagar gradualmente endpoints equivalentes del monolito.

Coexistencia sin downtime:

- Usar dual-read temporal desde Gateway: nueva ruta preferida, fallback al monolito para reservas antiguas.
- Usar migracion incremental por rangos de fecha o estado.
- Mantener `guid` como llave de compatibilidad.
- Congelar escrituras de una entidad durante una ventana corta si se migra su agregado.
- Aplicar outbox para reintentar integraciones fallidas.

Pruebas de integridad:

- Conteos por tabla antes/despues.
- Checksums por `guid`.
- Reservas activas sin duplicidad de habitacion y rango.
- Facturas con total igual a suma de detalles.
- Pagos no mayores al saldo salvo reglas explicitas.
- Estadias vinculadas a reserva confirmada.
- Valoraciones solo para estadias finalizadas.

---

## 10. Resultado esperado

Arquitectura final:

```text
Public Internet
   |
   v
Gateway.Api
   |
   +-- Auth.Api             -> hotel_auth
   +-- Clientes.Api         -> hotel_clientes
   +-- BookingPublic.Api    -> hotel_booking_read opcional
   +-- Alojamiento.Api      -> hotel_alojamiento
   +-- Tarifas.Api          -> hotel_tarifas
   +-- Reservas.Api         -> hotel_reservas
   +-- Estadias.Api         -> hotel_estadias
   +-- Facturacion.Api      -> hotel_facturacion
   +-- Pagos.Api            -> hotel_pagos
   +-- Valoraciones.Api     -> hotel_valoraciones
   +-- Auditoria.Api        -> hotel_auditoria
   +-- Notificaciones.Api   -> hotel_notificaciones
```

Buenas practicas aplicadas:

- DDD: dominios separados por bounded context real del hotel.
- Clean Architecture: cada servicio con `Api`, `Application`, `Domain`, `Infrastructure`.
- Database per service: autonomia de persistencia.
- GUIDs globales: integracion sin FKs fisicas.
- Outbox/eventos: consistencia eventual controlada.
- CQRS: lecturas publicas optimizadas y comandos transaccionales.
- Gateway central: autenticacion, autorizacion, routing, errores, logs y rate limiting.
- gRPC interno: disponibilidad, pricing, snapshots y operaciones de alta frecuencia.
- REST publico: contratos estables y amigables para clientes externos.
- Seguridad: microservicios internos no expuestos directamente.

La migracion no debe empezar separando tablas al azar. Debe seguir el flujo natural del sistema:

```text
Gateway -> Auth/Clientes -> Alojamiento/Tarifas/BookingPublic -> Reservas -> Facturacion/Pagos/Estadias/Valoraciones
```

Esta secuencia respeta las dependencias actuales observadas en el codigo: Auth depende de Clientes; Booking depende de Alojamiento, Tarifas, Reservas y Valoraciones; Reservas depende de Clientes, Habitaciones, Tarifas y Facturacion; Facturacion/Pagos/Estadias dependen de Reservas.

---

## Anexo A - Endpoints actuales clasificados

| Endpoint actual | Clasificacion | Servicio destino |
|---|---|---|
| `POST /api/v{version}/auth/login` | Publico | Auth |
| `POST /api/v{version}/auth/register-cliente` | Publico | Auth + Clientes |
| `POST /api/v{version}/auth/refresh` | Publico | Auth |
| `POST /api/v{version}/auth/logout` | Publico autenticado | Auth |
| `POST /api/v{version}/auth/cambiar-password` | Publico autenticado | Auth |
| `GET /api/v1/accommodations/search` | Publico | BookingPublic |
| `GET /api/v1/accommodations/{sucursalGuid}` | Publico | BookingPublic |
| `GET /api/v1/accommodations/{sucursalGuid}/reviews` | Publico | BookingPublic/Valoraciones |
| `GET /api/v1/accommodations/sucursales/{sucursalGuid}/habitaciones` | Publico | BookingPublic/Alojamiento |
| `POST /api/v1/accommodations/reservas` | Publico | BookingPublic/Reservas |
| `GET /api/v1/accommodations/reservas/{reservaGuid}` | Publico | BookingPublic/Reservas |
| `GET/POST /api/v{version}/public/clientes` | Publico | Clientes |
| `GET /api/v{version}/public/clientes/{clienteGuid}` | Publico | Clientes |
| `GET /api/v1/public/habitaciones` | Publico | Alojamiento |
| `GET /api/v1/public/habitaciones/{habitacionGuid}` | Publico | Alojamiento |
| `GET /api/v1/public/sucursales/{sucursalGuid}` | Publico | Alojamiento |
| `GET /api/v1/public/tipos-habitacion/{tipoHabitacionGuid}` | Publico | Alojamiento |
| `GET/POST/PATCH /api/v{version}/public/reservas` | Publico | Reservas |
| `POST /api/v{version}/public/pagos/simular` | Publico | Pagos |
| `/api/v{version}/internal/usuarios` | Interno | Auth |
| `/api/v{version}/internal/roles` | Interno | Auth |
| `/api/v{version}/internal/permisos` | Interno | Auth |
| `/api/v{version}/internal/auditoria` | Interno | Auditoria |
| `/api/v{version}/internal/sucursales` | Interno | Alojamiento |
| `/api/v{version}/internal/habitaciones` | Interno | Alojamiento |
| `/api/v{version}/internal/tipos-habitacion` | Interno | Alojamiento |
| `/api/v{version}/internal/catalogo-servicios` | Interno | Alojamiento |
| `/api/v{version}/internal/tarifas` | Interno | Tarifas |
| `/api/v{version}/internal/clientes` | Interno | Clientes |
| `/api/v{version}/internal/reservas` | Interno | Reservas |
| `/api/v{version}/internal/estadias` | Interno | Estadias |
| `/api/v{version}/internal/cargos-estadia` | Interno | Estadias |
| `/api/v{version}/internal/facturas` | Interno | Facturacion |
| `/api/v{version}/internal/pagos` | Interno | Pagos |
| `/api/v{version}/internal/valoraciones` | Interno | Valoraciones |

## Anexo B - Relaciones principales actuales

| Relacion SQL actual | Relacion propuesta |
|---|---|
| `RESERVAS.id_cliente -> CLIENTES.id_cliente` | `reservas.cliente_guid` externo |
| `RESERVAS.id_sucursal -> SUCURSAL.id_sucursal` | `reservas.sucursal_guid` externo |
| `RESERVAS_HABITACIONES.id_habitacion -> HABITACION.id_habitacion` | `reservas_habitaciones.habitacion_guid` externo |
| `RESERVAS_HABITACIONES.id_tarifa -> TARIFA.id_tarifa` | `reservas_habitaciones.tarifa_guid` externo opcional |
| `ESTADIAS.id_reserva_habitacion -> RESERVAS_HABITACIONES.id_reserva_habitacion` | `estadias.reserva_habitacion_guid` externo |
| `FACTURAS.id_reserva -> RESERVAS.id_reserva` | `facturas.reserva_guid` externo |
| `PAGOS.id_factura -> FACTURAS.id_factura` | `pagos.factura_guid` externo |
| `VALORACIONES.id_estadia -> ESTADIAS.id_estadia` | `valoraciones.estadia_guid` externo |
| `USUARIO_APP.id_cliente -> CLIENTES.id_cliente` | `usuarios.cliente_guid` externo |
