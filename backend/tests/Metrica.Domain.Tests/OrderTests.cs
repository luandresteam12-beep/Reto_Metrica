using Metrica.Domain.Entities;
using Metrica.Domain.Enums;
using Metrica.Domain.Exceptions;
using Xunit;

namespace Metrica.Domain.Tests;

public sealed class OrderTests
{
    [Fact]
    public void Create_ShouldRejectNonPositiveTotal()
    {
        var action = () => Order.Create("PED-001", "Juan Perez", DateTime.UtcNow, 0, OrderStatus.Registered);

        var exception = Assert.Throws<DomainRuleException>(action);
        Assert.Equal("El total del pedido debe ser mayor que cero.", exception.Message);
    }

    [Fact]
    public void Create_ShouldPreserveBusinessData()
    {
        var order = Order.Create(" PED-001 ", " Juan Perez ", new DateTime(2025, 1, 10), 250.75m, OrderStatus.Registered);

        Assert.Equal("PED-001", order.OrderNumber);
        Assert.Equal("Juan Perez", order.Customer);
        Assert.Equal(250.75m, order.Total);
        Assert.Equal(OrderStatus.Registered, order.Status);
        Assert.False(order.IsDeleted);
    }

    [Fact]
    public void SoftDelete_ShouldBeIdempotent()
    {
        var order = Order.Create("PED-001", "Juan Perez", DateTime.UtcNow, 10, OrderStatus.Registered);

        order.SoftDelete();
        var firstDeletionDate = order.DeletedAtUtc;
        order.SoftDelete();

        Assert.True(order.IsDeleted);
        Assert.Equal(firstDeletionDate, order.DeletedAtUtc);
    }
}
