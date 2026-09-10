using MediatR;
using Metrica.Application.Handlers.Commands.Orders;
using Metrica.Application.Handlers.Queries.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Metrica.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("api")]
[Route("api/pedidos")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? estado = null,CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetOrdersRequest
        {
            Search = search,
            Estado = estado
        }, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetOrderRequest { Id = id }, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,[FromBody] UpdateOrderRequest request,CancellationToken cancellationToken)
    {
        request.Id = id;
        var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderRequest { Id = id }, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }
}
