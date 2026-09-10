# Base de datos

`schema.sql` es un script idempotente generado desde la migración inicial de EF Core. También puede crearse la base aplicando las migraciones:

```powershell
dotnet ef database update --project ../backend/src/Metrica.Infrastructure/Metrica.Infrastructure.csproj --startup-project ../backend/src/Metrica.Api/Metrica.Api.csproj
```

La aplicación ejecuta `Database.MigrateAsync()` únicamente cuando `Database:AutoMigrate` está habilitado. Se recomienda dejarlo activo en desarrollo y gestionarlo desde un pipeline de despliegue en producción.
