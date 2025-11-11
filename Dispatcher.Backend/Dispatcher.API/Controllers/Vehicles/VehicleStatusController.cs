using Dispatcher.Application.Modules.Vehicles.TruckStatuses.Queries.GetById;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Create;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Delete;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Update;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.GetById;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.List;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleStatusesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create a new vehicle status.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> CreateVehicleStatus(CreateVehicleStatusCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);

        // Vraća 201 Created sa ID novog statusa
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }


    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Dispatcher")]
    public Task<GetVehicleStatusByIdQueryDto> GetById(int id,CancellationToken ct)
    {
        var status=sender.Send(new GetVehicleStatusByIdQuery { Id = id }, ct);
        return status;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Dispatcher")]
    public async Task<PageResult<ListVehicleStatusQueryDto>> List([FromQuery] ListVehicleStatusQuery query, CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return result;
    }


    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteVehicleStatusCommand { Id = id }, ct);
    }


    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task Update(int id, UpdateVehicleStatusCommand command, CancellationToken ct)
    {
        command.Id = id;

        await sender.Send(command, ct);
    }
}
