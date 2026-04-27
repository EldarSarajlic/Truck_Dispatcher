using Dispatcher.Application.Common;
using Dispatcher.Application.Modules.Inventory.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.API.Controllers.Inventory;

[ApiController]
[Route("[controller]")]
public class InventoryController(ISender sender) : ControllerBase
{
    /// <summary>List inventory items with optional search and category filter.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Dispatcher")]
    public async Task<ActionResult<PageResult<ListInventoryQueryDto>>> List(
        [FromQuery] ListInventoryQuery query, CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return Ok(result);
    }
}
