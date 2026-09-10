using Metrica.Domain.Enums;
using Metrica.Domain.Exceptions;

namespace Metrica.Domain.Entities;

public sealed class Order
{
    private Order()
    {
    }

    private Order(string orderNumber, string customer, DateTime orderDate, decimal total, OrderStatus status)
    {
        OrderNumber = orderNumber;
        Customer = customer;
        OrderDate = orderDate;
        Total = total;
        Status = status;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }

    public string OrderNumber { get; private set; } = string.Empty;

    public string Customer { get; private set; } = string.Empty;

    public DateTime OrderDate { get; private set; }

    public decimal Total { get; private set; }

    public OrderStatus Status { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public static Order Create(string orderNumber, string customer, DateTime orderDate, decimal total, OrderStatus status)
    {
        Validate(orderNumber, customer, orderDate, total);
        return new Order(orderNumber.Trim(), customer.Trim(), orderDate, total, status);
    }

    public void Update(string orderNumber, string customer, DateTime orderDate, decimal total, OrderStatus status)
    {
        if (IsDeleted)
        {
            throw new DomainRuleException("No se puede modificar un pedido eliminado.");
        }

        Validate(orderNumber, customer, orderDate, total);
        OrderNumber = orderNumber.Trim();
        Customer = customer.Trim();
        OrderDate = orderDate;
        Total = total;
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DeletedAtUtc;
    }

    private static void Validate(string orderNumber, string customer, DateTime orderDate, decimal total)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new DomainRuleException("El número de pedido es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(customer))
        {
            throw new DomainRuleException("El cliente es obligatorio.");
        }

        if (orderDate == default)
        {
            throw new DomainRuleException("La fecha del pedido es obligatoria.");
        }

        if (total <= 0)
        {
            throw new DomainRuleException("El total del pedido debe ser mayor que cero.");
        }
    }
}
