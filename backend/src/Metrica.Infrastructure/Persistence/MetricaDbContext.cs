using Metrica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metrica.Infrastructure.Persistence;

public sealed class MetricaDbContext(DbContextOptions<MetricaDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MetricaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
