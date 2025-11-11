//using Dispatcher.Application.Modules.Shipments.Commands.Create;
//using Dispatcher.Application.Modules.Shipments.Commands.Update;
//using Dispatcher.Application.Modules.Shipments.Commands.Delete;
//using Dispatcher.Application.Modules.Shipments.Queries.GetById;
//using Dispatcher.Application.Modules.Shipments.Queries.List;
using Dispatcher.Application.Modules.Shipments.Shipment.Querries.GetById;
using Dispatcher.Application.Modules.Shipments.Shipment.Querries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentsController(ISender sender) : ControllerBase
{
    // <summary>Create a new shipment.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]

    public async Task<ActionResult<int>> CreateShipment(CreateShipmentCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

   //  <summary>Get shipment by ID.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetShipmentByIdQueryDto>> GetById(int id, CancellationToken ct)
    {
        var shipment = await sender.Send(new GetShipmentByIdQuery { Id = id }, ct);
        if (shipment == null) return NotFound();
        return Ok(shipment);
    }

    /// <summary>List all shipments with optional filters.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PageResult<ListShipmentQueryDto>>> List([FromQuery] ListShipmentQuery query, CancellationToken ct)
    {
        var shipments = await sender.Send(query, ct);
        return Ok(shipments);
    }

    ///// <summary>Update existing shipment by ID.</summary>
    //[HttpPut("{id:int}")]
    //public async Task<ActionResult> Update(int id, UpdateShipmentCommand command, CancellationToken ct)
    //{
    //    if (id != command.Id) return BadRequest();
    //    await sender.Send(command, ct);
    //    return NoContent();
    //}

    ///// <summary>Delete shipment by ID.</summary>
    //[HttpDelete("{id:int}")]
    //public async Task<ActionResult> Delete(int id, CancellationToken ct)
    //{
    //    await sender.Send(new DeleteShipmentCommand { Id = id }, ct);
    //    return NoContent();
    //}
}
