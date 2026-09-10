using MediatR;
using Metrica.Application.Common.Exceptions;
using Metrica.Application.Handlers.Queries.Orders;
using Metrica.Application.Interfaces;
using Metrica.Application.Mapping;

namespace Metrica.Application.Handlers.Commands.Orders;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("el pedido", request.Id);

        if (await _orderRepository.ExistsByNumberAsync(request.NumeroPedido, request.Id, cancellationToken))
        {
            throw new ConflictException("Ya existe otro pedido con el mismo número.");
        }

        order.Update(
            request.NumeroPedido,
            request.Cliente,
            request.Fecha,
            request.Total,
            OrderMapping.ParseStatus(request.Estado));

        await _orderRepository.SaveChangesAsync(cancellationToken);
        return OrderMapping.ToResponse(order);
    }
}
