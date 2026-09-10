# Base de datos

`schema.sql` es un script manual de respaldo para una base `RetoMetrica` vacía. En el flujo normal no es necesario ejecutarlo: la API aplica la migración inicial automáticamente en Development.

Para aplicar migraciones desde EF Core:

```powershell
cd ../backend
dotnet ef database update --project src/Metrica.Infrastructure/Metrica.Infrastructure.csproj --startup-project src/Metrica.Api/Metrica.Api.csproj -- --environment Development
```

La aplicación ejecuta `Database.MigrateAsync()` únicamente cuando `Database:AutoMigrate` está habilitado. Se recomienda dejarlo activo en desarrollo y gestionarlo desde un pipeline de despliegue en producción.
