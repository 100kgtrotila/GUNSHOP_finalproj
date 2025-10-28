using Api.DTOs;
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
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateWeaponRequest request, CancellationToken ct)
    {
        var command = new CreateWeaponCommand
        {
            Name = request.Name,
            Manufacturer = request.Manufacturer,
            Model = request.Model,
            Caliber = request.Caliber,
            SerialNumber = request.SerialNumber,
            Price = request.Price,
            Category = request.Category
        };

        var weapon = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = weapon.Id },
            new { id = weapon.Id }
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WeaponResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var weapons = await _mediator.Send(new GetAllWeaponsQuery());

        var response = weapons.Select(w => new WeaponResponse(
            w.Id,
            w.Name,
            w.Manufacturer,
            w.Model,
            w.Caliber,
            w.SerialNumber,
            w.Price,
            w.Status,
            w.Category,
            w.CreatedAt,
            w.UpdatedAt
        ));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WeaponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var weapon = await _mediator.Send(new GetWeaponByIdQuery(id));

        if (weapon == null)
        {
            return NotFound();
        }

        var response = new WeaponResponse(
            weapon.Id,
            weapon.Name,
            weapon.Manufacturer,
            weapon.Model,
            weapon.Caliber,
            weapon.SerialNumber,
            weapon.Price,
            weapon.Status,
            weapon.Category,
            weapon.CreatedAt,
            weapon.UpdatedAt
        );

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeaponRequest request)
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

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeWeaponStatusRequest request)
    {
        var command = new ChangeWeaponStatusCommand
        {
            Id = id,
            NewStatus = request.Status
        };

        await _mediator.Send(command);
        return NoContent();
    }
}
