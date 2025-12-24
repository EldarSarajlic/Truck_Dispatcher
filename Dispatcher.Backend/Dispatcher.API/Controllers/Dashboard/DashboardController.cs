using Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dispatcher.Application.Modules.Dashboard.OrderStats.Summary.Queries.GetOrdersDashboardSummary;
namespace Dispatcher.API.Controllers;
using Dispatcher.Application.Modules.Dashboard.OrderStats.Charts.Queries.GetOrdersDashboardCharts;
using Dispatcher.Application.Modules.Dashboard.OrderStats.Reports.Queries.GetOrdersReport;
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

        /// <summary>
    /// Get orders dashboard KPI summary (current month snapshot).
    /// </summary>
    [HttpGet("orders/summary")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetOrdersDashboardSummaryQueryDto>> GetOrdersSummary(
        CancellationToken ct)
    {
        var result = await sender.Send(
            new GetOrdersDashboardSummaryQuery(), ct);

        return Ok(result);
    }

    [HttpGet("orders/charts")]
[Authorize(Roles = "Admin")]
public Task<GetOrdersDashboardChartsQueryDto> GetOrdersCharts(
    [FromQuery] int year,
    CancellationToken ct)
{
    var charts = sender.Send(
        new GetOrdersDashboardChartsQuery { Year = year }, ct);

    return charts;
}
[HttpGet("orders/report")]
[Authorize(Roles = "Admin")]
public Task<GetOrdersReportQueryDto> GetOrdersReport(
    [FromQuery] int year,
    [FromQuery] int? month,
    CancellationToken ct)
{
    var result=sender.Send(
        new GetOrdersReportQuery
        {
            Year = year,
            Month = month
        }, ct);

    return result;
}

}
