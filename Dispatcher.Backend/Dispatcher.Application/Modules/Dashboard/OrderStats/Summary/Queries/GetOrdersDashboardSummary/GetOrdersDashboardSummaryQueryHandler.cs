using Dispatcher.Application.Modules.Dashboard.OrderStats.Summary.Queries.GetOrdersDashboardSummary;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Summary.Queries.GetOrdersDashboardSummary
{
    public class GetOrdersDashboardSummaryQueryHandler(IAppDbContext context)
        : IRequestHandler<GetOrdersDashboardSummaryQuery, GetOrdersDashboardSummaryQueryDto>
    {
        public async Task<GetOrdersDashboardSummaryQueryDto> Handle(
            GetOrdersDashboardSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var currentYear = now.Year;
            var currentMonth = now.Month;

            var revenueStatuses = new[] { "Approved", "InProgress", "Completed" };

            // ===== TOTAL REVENUE (YEAR TO DATE) =====
            var totalRevenue = await context.Orders
                .AsNoTracking()
                .Where(o =>
                    o.OrderDate.Year == currentYear &&
                    revenueStatuses.Contains(o.Status))
                .SumAsync(o => o.TotalAmount, cancellationToken);

            // ===== CURRENT MONTH ORDERS =====
            var monthlyOrdersQuery = context.Orders
                .AsNoTracking()
                .Where(o =>
                    o.OrderDate.Year == currentYear &&
                    o.OrderDate.Month == currentMonth);

            var totalOrders = await monthlyOrdersQuery
                .CountAsync(cancellationToken);

            var monthlyRevenue = await monthlyOrdersQuery
                .Where(o => revenueStatuses.Contains(o.Status))
                .SumAsync(o => o.TotalAmount, cancellationToken);

            var avgOrderValue = totalOrders > 0
                ? await monthlyOrdersQuery.AverageAsync(o => o.TotalAmount, cancellationToken)
                : 0;

            return new GetOrdersDashboardSummaryQueryDto
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                TotalOrders = totalOrders,
                AvgOrderValue = avgOrderValue
            };
        }
    }
}
