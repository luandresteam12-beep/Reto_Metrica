using MediatR;

namespace Metrica.Application.Handlers.Queries.Orders;

public class GetOrderRequest : IRequest<OrderResponse>
{
    public int Id { get; set; }
}
