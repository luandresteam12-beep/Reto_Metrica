using MediatR;
using Metrica.Application.Common.Exceptions;
using Metrica.Application.Handlers.Queries.Orders;
using Metrica.Application.Interfaces;
using Metrica.Application.Mapping;
using Metrica.Domain.Entities;

namespace Metrica.Application.Handlers.Commands.Orders;

public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (await _orderRepository.ExistsByNumberAsync(request.NumeroPedido, null, cancellationToken))
        {
            throw new ConflictException("Ya existe un pedido con el mismo número.");
        }

        var order = Order.Create(
            request.NumeroPedido,
            request.Cliente,
            request.Fecha,
            request.Total,
            OrderMapping.ParseStatus(request.Estado));

        _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);
        return OrderMapping.ToResponse(order);
    }
}
