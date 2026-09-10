using Metrica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Metrica.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderNumber)
            .HasColumnName("NumeroPedido")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(order => order.Customer)
            .HasColumnName("Cliente")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(order => order.OrderDate)
            .HasColumnName("Fecha")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(order => order.Total)
            .HasColumnName("Total")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(order => order.Status)
            .HasColumnName("Estado")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(order => order.IsDeleted).HasColumnName("Eliminado");
        builder.Property(order => order.DeletedAtUtc).HasColumnName("FechaEliminacion");
        builder.Property(order => order.CreatedAtUtc).HasColumnName("CreadoEn");
        builder.Property(order => order.UpdatedAtUtc).HasColumnName("ActualizadoEn");
        builder.Property(order => order.RowVersion).HasColumnName("Version").IsRowVersion();

        builder.HasIndex(order => order.OrderNumber).IsUnique();
        builder.HasIndex(order => new { order.IsDeleted, order.OrderDate });
    }
}
