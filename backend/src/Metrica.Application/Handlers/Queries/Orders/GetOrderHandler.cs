using MediatR;
using Metrica.Application.Common.Exceptions;
using Metrica.Application.Interfaces;
using Metrica.Application.Mapping;

namespace Metrica.Application.Handlers.Queries.Orders;

public class GetOrderHandler : IRequestHandler<GetOrderRequest, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("el pedido", request.Id);

        return OrderMapping.ToResponse(order);
    }
}
