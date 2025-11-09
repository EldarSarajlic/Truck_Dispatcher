using Dispatcher.Application.Modules.Vehicles.Trucks.Commands;
using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Create;
using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Delete;
using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Disable;
using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Enable;
using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Update;
using Dispatcher.Application.Modules.Vehicles.Trucks.Queries.GetById;
using Dispatcher.Application.Modules.Vehicles.Trucks.Queries.List;
using Dispatcher.Application.Modules.Vehicles.Trucks.Querries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrucksController(ISender sender) : ControllerBase
{
    /// <summary>Create a new truck.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<int>> CreateTruck(CreateTruckCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>Get truck by ID.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<GetTruckByIdQueryDto>> GetById(int id, CancellationToken ct)
    {
        var truck = await sender.Send(new GetTruckByIdQuery { Id = id }, ct);
        if (truck == null) return NotFound();
        return Ok(truck);
    }

    /// <summary>List all trucks, with optional search/filter.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<List<ListTruckQueryDto>>> List([FromQuery] ListTruckQuery query, CancellationToken ct)
    {
        var trucks = await sender.Send(query, ct);
        return Ok(trucks);
    }

    /// <summary>Update an existing truck by ID.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult> Update(int id, UpdateTruckCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>Delete a truck by ID.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteTruckCommand { Id = id }, ct);
        return NoContent();
    }

    /// <summary>Enable a truck (change status).</summary>
    [HttpPost("{id:int}/enable")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult> Enable(int id, CancellationToken ct)
    {
        await sender.Send(new EnableTruckCommand { Id = id }, ct);
        return NoContent();
    }

    /// <summary>Disable a truck (change status).</summary>
    [HttpPost("{id:int}/disable")]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult> Disable(int id, CancellationToken ct)
    {
        await sender.Send(new DisableTruckCommand { Id = id }, ct);
        return NoContent();
    }
}
