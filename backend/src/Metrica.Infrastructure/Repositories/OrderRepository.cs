using Metrica.Application.Interfaces;
using Metrica.Domain.Entities;
using Metrica.Domain.Enums;
using Metrica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Metrica.Infrastructure.Repositories;

public sealed class OrderRepository(
    MetricaDbContext dbContext,
    ResiliencePipeline resiliencePipeline) : IOrderRepository
{
    private readonly ResiliencePipeline _resiliencePipeline = resiliencePipeline;

    public async Task<IReadOnlyList<Order>> GetAllAsync(
        string? search,
        OrderStatus? status,
        CancellationToken cancellationToken)
    {
        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var query = dbContext.Orders.AsNoTracking().Where(order => !order.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(order => order.OrderNumber.Contains(term) || order.Customer.Contains(term));
            }

            if (status.HasValue)
            {
                query = query.Where(order => order.Status == status.Value);
            }

            var items = await query
                .OrderByDescending(order => order.OrderDate)
                .ThenByDescending(order => order.Id)
                .ToListAsync(token);

            return (IReadOnlyList<Order>)items;
        }, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _resiliencePipeline.ExecuteAsync(
            async token => await dbContext.Orders.FirstOrDefaultAsync(order => order.Id == id && !order.IsDeleted, token),
            cancellationToken);
    }

    public async Task<bool> ExistsByNumberAsync(string orderNumber, int? excludingId, CancellationToken cancellationToken)
    {
        var normalizedNumber = orderNumber.Trim();
        return await _resiliencePipeline.ExecuteAsync(
            async token => await dbContext.Orders.AsNoTracking().AnyAsync(
                order => !order.IsDeleted && order.OrderNumber == normalizedNumber && (!excludingId.HasValue || order.Id != excludingId.Value),
                token),
            cancellationToken);
    }

    public void Add(Order order) => dbContext.Orders.Add(order);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _resiliencePipeline.ExecuteAsync(
            async token => await dbContext.SaveChangesAsync(token),
            cancellationToken);
    }
}
