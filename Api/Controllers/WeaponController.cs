using Api.DTOs;
using Application.Comments.Commands;
using Application.Comments.Queries;
using Application.Weapons.Commands;
using Application.Weapons.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/weapons")]
public class WeaponController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeaponController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(WeaponDetailedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateWeaponRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateWeaponCommand
        {
            Name = request.Name, Manufacturer = request.Manufacturer, Model = request.Model,
            Caliber = request.Caliber, SerialNumber = request.SerialNumber, Price = request.Price,
            Category = request.Category
        };
    
        var weapon = await _mediator.Send(command, cancellationToken);

        var response = new WeaponDetailedResponse(
            weapon.Id, weapon.Name, weapon.Manufacturer, weapon.Model,
            weapon.Caliber, weapon.SerialNumber, weapon.Price, weapon.Status,
            weapon.Category, weapon.CreatedAt, weapon.UpdatedAt,
            new List<ProductCommentDto>()
        );
    
        return CreatedAtAction(nameof(GetById), new { id = weapon.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteWeaponCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WeaponDetailedResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var weapons = await _mediator.Send(new GetAllWeaponsQuery(), cancellationToken);
        var response = weapons.Select(w => new WeaponDetailedResponse(
            w.Id, w.Name, w.Manufacturer, w.Model, w.Caliber, w.SerialNumber,
            w.Price, w.Status, w.Category, w.CreatedAt, w.UpdatedAt,
            new List<ProductCommentDto>()
        ));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WeaponDetailedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var weapon = await _mediator.Send(new GetWeaponByIdQuery(id), cancellationToken);
        if (weapon == null)
        {
            return NotFound();
        }

        var response = new WeaponDetailedResponse(
            weapon.Id, weapon.Name, weapon.Manufacturer, weapon.Model,
            weapon.Caliber, weapon.SerialNumber, weapon.Price, weapon.Status,
            weapon.Category, weapon.CreatedAt, weapon.UpdatedAt,
            weapon.Comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new ProductCommentDto(c.Id, c.Content, c.CreatedAt, c.UpdatedAt)) 
                .ToList()
        );
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeaponRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateWeaponCommand
        {
            Id = id,
            Name = request.Name,
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            Caliber = request.Caliber,
            Price = request.Price
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeWeaponStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeWeaponStatusCommand
        {
            Id = id,
            NewStatus = request.Status
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("{weaponId:guid}/comments")]
    [ProducesResponseType(typeof(IEnumerable<ProductCommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommentsForWeapon(Guid weaponId, CancellationToken cancellationToken)
    {
        var query = new GetCommentsByWeaponIdQuery(weaponId);
        var comments = await _mediator.Send(query, cancellationToken);

        var response = comments.Select(c => new ProductCommentDto(c.Id, c.Content, c.CreatedAt, c.UpdatedAt));
        return Ok(response);
    }

    [HttpPost("{weaponId:guid}/comments")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCommentForWeapon(Guid weaponId, [FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(weaponId, request.Content);
        var commentId = await _mediator.Send(command, cancellationToken);

        return Created($"api/comments/{commentId}", new { id = commentId });
    }
}
