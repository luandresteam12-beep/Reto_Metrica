using System.ComponentModel.DataAnnotations;
using MediatR;
using Metrica.Application.Handlers.Queries.Orders;

namespace Metrica.Application.Handlers.Commands.Orders;

public class CreateOrderRequest : IRequest<OrderResponse>
{
    [Required, StringLength(50)]
    public string NumeroPedido { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Cliente { get; set; } = string.Empty;

    [Required]
    public DateTime Fecha { get; set; }

    [Range(0.01, 999999999999.99)]
    public decimal Total { get; set; }

    [Required, StringLength(30)]
    public string Estado { get; set; } = "Registrado";
}
