using MediatR;

namespace Metrica.Application.Handlers.Commands.Orders;

public class DeleteOrderRequest : IRequest<Unit>
{
    public int Id { get; set; }
}
