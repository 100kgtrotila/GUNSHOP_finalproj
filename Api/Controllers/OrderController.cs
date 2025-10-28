using Api.DTOs;
using Application.Orders.Commands;
using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand
        {
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            WeaponId = request.WeaponId,
            TotalAmount = request.TotalAmount,
            OrderDate = request.OrderDate
        };

        var order = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            new { id = order.Id }
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());

        var response = orders.Select(o => new OrderResponse(
            o.Id,
            o.OrderNumber,
            o.CustomerId,
            o.WeaponId,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.Notes,
            o.CreatedAt,
            o.UpdatedAt
        ));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));

        if (order == null)
        {
            return NotFound();
        }

        var response = new OrderResponse(
            order.Id,
            order.OrderNumber,
            order.CustomerId,
            order.WeaponId,
            order.OrderDate,
            order.Status,
            order.TotalAmount,
            order.Notes,
            order.CreatedAt,
            order.UpdatedAt
        );

        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
    {
        var command = new UpdateOrderStatusCommand
        {
            Id = id,
            NewStatus = request.Status,
            Notes = request.Notes
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteOrderRequest request)
    {
        var command = new CompleteOrderCommand
        {
            Id = id,
            CompletionNotes = request.CompletionNotes
        };

        await _mediator.Send(command);
        return NoContent();
    }
}
