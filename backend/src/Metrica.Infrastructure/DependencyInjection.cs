using Metrica.Application.Interfaces;
using Metrica.Infrastructure.Persistence;
using Metrica.Infrastructure.Repositories;
using Metrica.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Metrica.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Debe configurarse ConnectionStrings:DefaultConnection.");
        }

        services.AddDbContext<MetricaDbContext>(options => options
            .UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));

        services.AddOptions<AuthOptions>()
            .Bind(configuration.GetSection("Auth"))
            .Validate(options => !string.IsNullOrWhiteSpace(options.AdminEmail), "Auth:AdminEmail es obligatorio.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.AdminPassword), "Auth:AdminPassword es obligatorio.")
            .ValidateOnStart();

        services.AddSingleton(SqlResiliencePolicy.Create());

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAdminCredentialsValidator, AdminCredentialsValidator>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
