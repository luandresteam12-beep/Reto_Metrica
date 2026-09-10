# Reto técnico Fullstack Senior — Metrica Orders

Aplicación end-to-end para autenticación y gestión de pedidos, construida con .NET 8, SQL Server, Entity Framework Core, React y TypeScript.

## Alcance implementado

- Login con JWT Bearer y claims de usuario/rol.
- Login simple contra credenciales configuradas (`Auth:AdminEmail` y `Auth:AdminPassword`); no se persisten usuarios en la base de datos.
- CRUD completo de pedidos.
- Eliminación lógica; solo el rol `Admin` puede eliminar.
- Validaciones de entrada con DataAnnotations y reglas de dominio.
- Número de pedido único reforzado en aplicación y base de datos.
- Rate limiting por IP para login y operaciones de API.
- Reintentos transitorios y circuit breaker con Polly para operaciones SQL.
- Manejo global de excepciones con `ProblemDetails` y `traceId`.
- EF Core con SQL Server, migración inicial y migración automática configurable.
- Swagger con esquema Bearer.
- Frontend React por features, rutas protegidas, React Query, React Hook Form y Zod.
- Pruebas unitarias de las reglas principales del dominio.

## Estructura

```text
Reto_Metrica/
├── backend/
│   ├── Metrica.sln
│   ├── src/
│   │   ├── Metrica.Api
│   │   ├── Metrica.Application
│   │   ├── Metrica.Domain
│   │   └── Metrica.Infrastructure
│   └── tests/Metrica.Domain.Tests
├── frontend/
│   └── src/
│       ├── app/                 # rutas y layout de la aplicación
│       ├── features/
│       │   ├── auth/            # domain, application, infrastructure, presentation
│       │   └── orders/          # domain, application, infrastructure, presentation
│       └── shared/              # cliente HTTP, errores y tipos compartidos
└── database/schema.sql
```

El frontend usa una Clean Architecture ligera por feature: `domain` contiene los contratos, `application` los hooks y validaciones, `infrastructure` el consumo HTTP y `presentation` las páginas y componentes. Así las pantallas se mantienen pequeñas sin crear capas innecesarias.

La capa `Metrica.Application` conserva la organización base del `UserService`: `Common`, `Handlers/Commands`, `Handlers/Queries`, `Interfaces`, `Mapping` y `DependencyInjection.cs`. MediatR se registra desde esa capa y los controllers solo reciben la request, la envían al handler y devuelven la respuesta.

## Requisitos

- .NET SDK 8.
- Node.js 18 o superior.
- SQL Server local, SQL Server Express, LocalDB o una instancia accesible.

## Configuración del backend

Después de clonar el repositorio, configura los valores locales en `appsettings.Development.json`. Este archivo no se guarda en Git:

Ubica `backend/src/Metrica.Api/appsettings.Development.example.json`, crea una copia llamada `appsettings.Development.json` y completa estos valores localmente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "TU_CADENA_DE_CONEXION"
  },
  "Jwt": {
    "Key": "TU_CLAVE_JWT_DE_AL_MENOS_32_CARACTERES"
  },
  "Auth": {
    "AdminEmail": "admin@metrica.local",
    "AdminPassword": "TU_CONTRASENA_LOCAL"
  }
}
```

La conexión, la clave JWT y las credenciales no deben subirse al repositorio. El archivo `appsettings.Development.json` está excluido mediante `.gitignore`. La aplicación ejecuta las migraciones automáticamente en Development mediante `Database.MigrateAsync()` cuando `Database:AutoMigrate` está habilitado.

Desde `backend/`, la aplicación puede aplicar la migración automáticamente al iniciar. Como alternativa manual:

```powershell
dotnet ef database update --project src/Metrica.Infrastructure/Metrica.Infrastructure.csproj --startup-project src/Metrica.Api/Metrica.Api.csproj -- --environment Development
```

Ejecuta la API desde `backend/` usando el perfil HTTP de desarrollo:

```powershell
dotnet run --project src/Metrica.Api/Metrica.Api.csproj --launch-profile http
```

- Swagger: `http://localhost:5248/swagger`
- API: `http://localhost:5248`
- Configura el correo y contraseña administrativos que usarás localmente. Estos valores no se almacenan en el repositorio.

## Configuración del frontend

Desde `frontend/`:

```powershell
npm ci
Copy-Item .env.example .env
npm run dev
```

El ejemplo usa el perfil HTTP del backend: `http://localhost:5248`. Si levantas el perfil HTTPS, usa `https://localhost:7248` y acepta el certificado local de desarrollo.

## Endpoints

| Método | Ruta | Seguridad |
|---|---|---|
| POST | `/auth/login` | Público, rate limited |
| GET | `/api/pedidos` | JWT |
| GET | `/api/pedidos/{id}` | JWT |
| POST | `/api/pedidos` | JWT |
| PUT | `/api/pedidos/{id}` | JWT |
| DELETE | `/api/pedidos/{id}` | JWT + rol Admin |

Los endpoints de pedidos requieren un JWT con rol `Admin`. El listado acepta `search` y `estado`. Los estados soportados son `Registrado`, `Confirmado`, `Enviado`, `Entregado` y `Cancelado`.

### Ejemplo de login

```json
POST /auth/login
{
  "email": "<CORREO_ADMIN>",
  "password": "<CONTRASENA_ADMIN>"
}
```

La respuesta incluye `token` y `expiresIn`. En Swagger pulsa `Authorize` y envía `Bearer {token}` para probar los endpoints protegidos.

### Ejemplo de pedido

```json
{
  "numeroPedido": "PED-001",
  "cliente": "Juan Perez",
  "fecha": "2026-09-09",
  "total": 250.75,
  "estado": "Registrado"
}
```

## Verificación

```powershell
dotnet build backend/Metrica.sln
dotnet test backend/Metrica.sln
```

Para el frontend:

```powershell
cd frontend
npm run build
npm run lint
```

## Pruebas manuales recomendadas

Con la API y el frontend levantados, verifica este flujo mínimo:

1. Iniciar sesión con el correo y contraseña configurados en `appsettings.Development.json` y confirmar que se recibe un JWT.
2. Intentar acceder a pedidos sin autenticación y confirmar `401 Unauthorized`.
3. Crear un pedido válido y comprobar la respuesta `201 Created`.
4. Editar el pedido y comprobar la respuesta `200 OK`.
5. Buscar por número, cliente y estado desde el listado.
6. Intentar crear un número de pedido duplicado y confirmar `409 Conflict`.
7. Intentar guardar un total menor o igual a cero y confirmar `400 Bad Request`.
8. Eliminar un pedido y confirmar `204 No Content`; el registro debe permanecer en SQL Server con `Eliminado = 1` y no aparecer en el listado.
9. Repetir más de cinco intentos de login en un minuto y confirmar `429 Too Many Requests`.
10. Cerrar sesión, refrescar la aplicación y confirmar que las rutas protegidas vuelven al login.

Para revisar la persistencia:

```sql
USE RetoMetrica;
SELECT * FROM dbo.Pedidos ORDER BY Id DESC;
SELECT * FROM dbo.__EFMigrationsHistory;
```

## Logging, resiliencia y control de tráfico

- `AddJsonConsole()` habilita logs estructurados en la consola del backend.
- `ExceptionHandlingMiddleware` registra excepciones con `traceId` y responde con `ProblemDetails` sin exponer stack traces.
- El login tiene un límite de 5 solicitudes por minuto por IP.
- La API de pedidos tiene un límite de 120 solicitudes por minuto por IP.
- Entity Framework usa reintentos para fallos transitorios de SQL Server.
- Polly agrega reintentos y Circuit Breaker para las operaciones del repositorio.
- El Circuit Breaker se abre después de cinco operaciones con al menos 50% de fallos, permanece abierto 20 segundos y devuelve `503` cuando corresponde.

## Decisiones de seguridad

- La cadena de conexión, la clave JWT y las credenciales de `Auth` se configuran en el `appsettings.Development.json` local, excluido mediante `.gitignore`; nunca deben subirse al repositorio.
- El token se mantiene en `sessionStorage` y se elimina cuando expira la sesión; el backend siempre es la autoridad.
- CORS acepta únicamente los orígenes configurados.
- El backend no retorna ni persiste hashes de usuarios; la única identidad de acceso se configura fuera de la tabla `Pedidos`.
- Las excepciones no exponen stack traces; se entrega un `traceId` para correlación en logs.
- La validación de frontend mejora UX, pero toda regla crítica se valida nuevamente en backend y base de datos.
