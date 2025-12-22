using Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview
{
    public class GetAdminDashboardOverviewQueryHandler(IAppDbContext context)
        : IRequestHandler<GetAdminDashboardOverviewQuery, GetAdminDashboardOverviewQueryDto>
    {
        public async Task<GetAdminDashboardOverviewQueryDto> Handle(
            GetAdminDashboardOverviewQuery request,
            CancellationToken cancellationToken)
        {
            var revenueStatuses = new[] { "Approved", "InProgress", "Completed" };
            var totalSales = await context.Orders
    .AsNoTracking()
    .Where(o => revenueStatuses.Contains(o.Status))
    .SumAsync(o => o.TotalAmount, cancellationToken);

            var totalOrders = await context.Orders
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var pendingOrders = await context.Orders
                .AsNoTracking()
                .CountAsync(o => o.Status == "Pending", cancellationToken);

            var totalUsers = await context.Users
                .AsNoTracking()
                .CountAsync(cancellationToken);

var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

var recentOrders = await context.Orders
    .AsNoTracking()
    .Include(o => o.Client)
    .Where(o => o.OrderDate >= oneWeekAgo)
    .OrderByDescending(o => o.OrderDate)
    .Take(5)
    .Select(o => new RecentOrderDto
    {
        Reference = o.OrderNumber,
        Client = o.Client.DisplayName, // prilagodi ako treba
        Price = o.TotalAmount,
        Date = o.OrderDate,
        Status=o.Status
    })
    .ToListAsync(cancellationToken);

            return new GetAdminDashboardOverviewQueryDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                TotalUsers = totalUsers,
                PendingOrders = pendingOrders,
                RecentOrders = recentOrders
            };
        }
    }
}
