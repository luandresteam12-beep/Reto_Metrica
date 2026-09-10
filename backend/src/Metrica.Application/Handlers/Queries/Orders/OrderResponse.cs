namespace Metrica.Application.Handlers.Queries.Orders;

public class OrderResponse
{
    public int Id { get; set; }

    public string NumeroPedido { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = string.Empty;
}
