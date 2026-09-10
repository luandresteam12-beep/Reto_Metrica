using Metrica.Domain.Entities;
using Metrica.Domain.Enums;

namespace Metrica.Application.Interfaces;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync(string? search, OrderStatus? status, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> ExistsByNumberAsync(string orderNumber, int? excludingId, CancellationToken cancellationToken);

    void Add(Order order);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
