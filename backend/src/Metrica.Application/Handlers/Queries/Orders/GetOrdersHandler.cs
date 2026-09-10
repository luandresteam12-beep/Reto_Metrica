using MediatR;
using Metrica.Application.Interfaces;
using Metrica.Application.Mapping;
using Metrica.Domain.Enums;

namespace Metrica.Application.Handlers.Queries.Orders;

public class GetOrdersHandler : IRequestHandler<GetOrdersRequest, IReadOnlyList<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var status = string.IsNullOrWhiteSpace(request.Estado)
            ? (OrderStatus?)null
            : OrderMapping.ParseStatus(request.Estado);

        var orders = await _orderRepository.GetAllAsync(request.Search, status, cancellationToken);
        return orders.Select(OrderMapping.ToResponse).ToList();
    }
}
