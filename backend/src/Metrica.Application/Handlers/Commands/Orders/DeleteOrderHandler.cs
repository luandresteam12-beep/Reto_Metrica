using MediatR;
using Metrica.Application.Common.Exceptions;
using Metrica.Application.Interfaces;

namespace Metrica.Application.Handlers.Commands.Orders;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, Unit>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("el pedido", request.Id);

        order.SoftDelete();
        await _orderRepository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
