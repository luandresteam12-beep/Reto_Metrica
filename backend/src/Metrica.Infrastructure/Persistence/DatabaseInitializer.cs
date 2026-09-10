using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metrica.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (!configuration.GetValue("Database:AutoMigrate", app.Environment.IsDevelopment()))
        {
            return;
        }

        var dbContext = scope.ServiceProvider.GetRequiredService<MetricaDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
