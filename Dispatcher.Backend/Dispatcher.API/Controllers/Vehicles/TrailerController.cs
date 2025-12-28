using Dispatcher.Application.Modules.Vehicles.Trailers.Commands;
using Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Create;
using Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Delete;
using Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Status.Change;
using Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Update;
using Dispatcher.Application.Modules.Vehicles.Trailers.Queries.GetById;
using Dispatcher.Application.Modules.Vehicles.Trailers.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrailersController(ISender sender) : ControllerBase
{
    /// <summary>Create a new trailer.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> CreateTrailer(
        CreateTrailerCommand command,
        CancellationToken ct)
    {
        int id = await sender.Send(command, ct);

        
        return CreatedAtAction(nameof(CreateTrailer), new { id }, new { id });
    }

    /// <summary>Delete a trailer by ID.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteTrailerCommand { Id = id }, ct);
        return NoContent();
    }

[HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin,Dispatcher")]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] ChangeTrailerStatusCommand request,
        CancellationToken ct)
    {
        await sender.Send(new ChangeTrailerStatusCommand
        {
            TrailerId = id,
            VehicleStatusId = request.VehicleStatusId
        }, ct);

        return Ok();
    }



[HttpPut("{id:int}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Update(
    int id,
    UpdateTrailerCommand command,
    CancellationToken ct)
{
    command.Id = id; 
    await sender.Send(command, ct);
    return NoContent();
}


[HttpGet]
[Authorize(Roles = "Admin,Dispatcher")]
public async Task<ActionResult<PageResult<ListTrailerQueryDto>>> List(
    [FromQuery] ListTrailerQuery query,
    CancellationToken ct)
{
    var trailers = await sender.Send(query, ct);
    return Ok(trailers);
}

    /// <summary>Get trailer by ID.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Dispatcher")]

    public async Task<ActionResult<GetTrailerByIdQueryDto>> GetById(int id, CancellationToken ct)
    {
        var trailer = await sender.Send(new GetTrailerByIdQuery { Id = id }, ct);
        if (trailer == null) return NotFound();
        return Ok(trailer);
    }


}
