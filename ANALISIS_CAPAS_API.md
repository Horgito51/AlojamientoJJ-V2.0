# Analisis de capas y contratos HTTP

## Resumen

Se reviso el flujo `API -> Business -> DataManagement -> DataAccess` con foco en:

- alineacion de rutas con el catalogo de endpoints esperado
- acoplamiento entre DTOs de negocio y contratos HTTP
- existencia real de controladores por modulo
- continuidad vertical de las capas para `sucursales` y `catalogo-servicios`

## Hallazgos principales

1. La API exponia varios endpoints con rutas singulares derivadas de `[controller]`, por lo que no coincidían con el contrato esperado (`usuarios`, `roles`, `habitaciones`, `tarifas`, etc.).
2. Los controladores estaban usando el mismo DTO de `Business` para crear, actualizar y responder. Eso mezclaba:
   - campos auditables
   - identificadores internos
   - `RowVersion`
   - datos de solo lectura
3. Existian controladores vacios o duplicados:
   - `AccommodationsControler.cs`
   - `Internal/Facturacion/PagoController.cs`
   - `SucursalController.cs`
   - `CatalogoServicioController.cs`
4. `sucursales` y `catalogo-servicios` tenian repositorios y entidades, pero no tenian subida completa a `DataManagement` ni `Business`.
5. La solucion no compilaba por incompatibilidad entre `IDictionary<string,string[]>` y `Dictionary<string,string[]>` en el middleware de excepciones.

## Cambios aplicados

### API

- Se normalizaron rutas para que coincidan con el catalogo esperado.
- Se agregaron contratos HTTP de entrada en `Servicio.Hotel.API/Models/Requests/Internal/`.
- Se dejaron de recibir strings crudos en varios `PATCH` y `POST`.
- Se eliminaron controladores duplicados/vacios que generaban ruido.

### Business

- Se agregaron DTOs, interfaces, mappers y services para:
  - `Sucursal`
  - `CatalogoServicio`

### DataManagement

- Se agregaron modelos, interfaces, mappers y data services para:
  - `Sucursal`
  - `CatalogoServicio`

### Wiring / infraestructura

- Se registraron nuevos repositorios, data services y business services en DI.
- Se corrigio el middleware de excepciones.
- Se agregaron validaciones de configuracion en `Program.cs`.

## Estado de cobertura de endpoints

### API publica

- `GET /api/v1/accommodations/search`
- `GET /api/v1/accommodations/{id}`
- `GET /api/v1/accommodations/{id}/reviews`

### API interna

- `POST /api/v1/internal/auth/login`
- `POST /api/v1/internal/auth/refresh`
- `POST /api/v1/internal/auth/logout`
- `GET|POST|GET by id|PUT|PATCH inhabilitar|DELETE /api/v1/internal/usuarios`
- `GET|POST|GET by id|PUT|DELETE /api/v1/internal/roles`
- `GET|POST|GET by id|PUT|DELETE /api/v1/internal/sucursales`
- `GET|POST|GET by id|PUT|PATCH estado|DELETE /api/v1/internal/habitaciones`
- `GET|POST|GET by id|PUT|PATCH desactivar|DELETE /api/v1/internal/tarifas`
- `GET|POST|GET by id|PUT|DELETE /api/v1/internal/tipos-habitacion`
- `GET|POST|GET by id|PUT|DELETE /api/v1/internal/catalogo-servicios`
- `GET|POST|GET by id|PATCH confirmar|PATCH cancelar /api/v1/internal/reservas`
- `GET|POST|GET by id|PUT|DELETE /api/v1/internal/clientes`
- `POST checkin|PATCH checkout|GET|GET by id /api/v1/internal/estadias`
- `GET cargos|POST cargos /api/v1/internal/estadias/{id}/cargos`
- `PATCH /api/v1/internal/cargos-estadia/{id}/anular`
- `GET|POST generar-reserva|POST generar-final|GET by id|PATCH anular /api/v1/internal/facturas`
- `GET|POST|GET by id|PATCH estado /api/v1/internal/pagos`
- `GET|POST|GET by id|PATCH moderar|PATCH responder|DELETE /api/v1/internal/valoraciones`

## Riesgos que siguen vigentes

1. En `Business` aun predominan DTOs unicos por agregado. La API ya no los expone directamente para escritura en los modulos ajustados, pero internamente siguen sirviendo como DTO mixto.
2. Faltaria una segunda refactorizacion para separar `CreateDTO`, `UpdateDTO` y `ResponseDTO` dentro de `Business` si quieres una arquitectura mas estricta.
3. Persisten muchos warnings historicos de nulabilidad en modelos y mappers; se resolvieron los de la API para dejar el borde compilando limpio.

## Recomendacion siguiente

Como siguiente iteracion conviene atacar por modulo:

1. `Seguridad`
2. `Reservas`
3. `Alojamiento`
4. `Facturacion`
5. `Hospedaje`
6. `Valoraciones`

Y en cada uno separar explicitamente:

- request de creacion
- request de actualizacion
- response
- filtros
- comandos de cambio de estado
