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
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand
        {
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            WeaponId = request.WeaponId,
            TotalAmount = request.TotalAmount,
            OrderDate = request.OrderDate
        };
    
        var order = await _mediator.Send(command, cancellationToken);

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
    
        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            response 
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery(), cancellationToken);
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
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) 
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken); 
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
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken) 
    {
        var command = new UpdateOrderStatusCommand
        {
            Id = id,
            NewStatus = request.Status,
            Notes = request.Notes
        };
        await _mediator.Send(command, cancellationToken);  
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteOrderRequest request, CancellationToken cancellationToken) 
    {
        var command = new CompleteOrderCommand
        {
            Id = id,
            CompletionNotes = request.CompletionNotes
        };
        await _mediator.Send(command, cancellationToken); 
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);
        return NoContent();
    }

}
