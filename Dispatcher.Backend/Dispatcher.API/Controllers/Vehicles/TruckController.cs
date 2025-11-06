//using Dispatcher.Application.Modules.Vehicles.Trucks.Commands;
//using Dispatcher.Application.Modules.Vehicles.TruckStatuses.Queries;
//using MediatR; // tj. ISender

using Dispatcher.Application.Modules.Vehicles.Trucks.Querries.GetById;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrucksController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create a new truck.
    /// </summary>
    //[HttpPost]
    //public async Task<ActionResult<int>> CreateTruck(CreateTruckCommand command, CancellationToken ct)
    //{
    //    int id = await sender.Send(command, ct);
    //    return CreatedAtAction(nameof(GetById), new { id }, new { id });
    //}

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetTruckByIdQueryDto>> GetById(int id, CancellationToken ct)
    {
        var truck = await sender.Send(new GetTruckByIdQuery { Id = id }, ct);
        if (truck == null) return NotFound();
        return Ok(truck);
    }

}