using MediatR;

namespace Metrica.Application.Handlers.Queries.Orders;

public class GetOrdersRequest : IRequest<IReadOnlyList<OrderResponse>>
{
    public string? Search { get; set; }

    public string? Estado { get; set; }
}
