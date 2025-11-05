using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Create;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleStatusesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create a new vehicle status.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<int>> CreateVehicleStatus(CreateVehicleStatusCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);

        // Vraća 201 Created sa ID novog statusa
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    // Optional: placeholder za GetById da bi CreatedAtAction radio
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        // Ovdje možeš implementirati kasnije Query za GetById
        return Ok();
    }
}
