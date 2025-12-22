using Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Get admin dashboard overview (system snapshot).
    /// </summary>
    [HttpGet("overview")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetAdminDashboardOverviewQueryDto>> GetOverview(
        CancellationToken ct)
    {
        var result = await sender.Send(
            new GetAdminDashboardOverviewQuery(), ct);

        return Ok(result);
    }
}
