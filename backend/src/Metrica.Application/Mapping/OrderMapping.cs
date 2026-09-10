using Metrica.Application.Handlers.Queries.Orders;
using Metrica.Domain.Entities;
using Metrica.Domain.Enums;
using Metrica.Domain.Exceptions;

namespace Metrica.Application.Mapping;

public static class OrderMapping
{
    public static OrderResponse ToResponse(Order order) => new()
    {
        Id = order.Id,
        NumeroPedido = order.OrderNumber,
        Cliente = order.Customer,
        Fecha = order.OrderDate,
        Total = order.Total,
        Estado = order.Status switch
        {
            OrderStatus.Registered => "Registrado",
            OrderStatus.Confirmed => "Confirmado",
            OrderStatus.Shipped => "Enviado",
            OrderStatus.Delivered => "Entregado",
            OrderStatus.Cancelled => "Cancelado",
            _ => order.Status.ToString()
        }
    };

    public static OrderStatus ParseStatus(string? value)
    {
        var normalized = value?.Trim().ToLowerInvariant();
        return normalized switch
        {
            "registrado" or "registered" => OrderStatus.Registered,
            "confirmado" or "confirmed" => OrderStatus.Confirmed,
            "enviado" or "shipped" => OrderStatus.Shipped,
            "entregado" or "delivered" => OrderStatus.Delivered,
            "cancelado" or "cancelled" or "canceled" => OrderStatus.Cancelled,
            _ => throw new DomainRuleException("El estado indicado no es válido.")
        };
    }
}
