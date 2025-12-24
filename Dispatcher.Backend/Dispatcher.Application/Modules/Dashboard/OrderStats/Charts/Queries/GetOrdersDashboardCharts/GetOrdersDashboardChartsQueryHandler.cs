using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Charts.Queries.GetOrdersDashboardCharts
{
    public class GetOrdersDashboardChartsQueryHandler(IAppDbContext context)
        : IRequestHandler<GetOrdersDashboardChartsQuery, GetOrdersDashboardChartsQueryDto>
    {
        public async Task<GetOrdersDashboardChartsQueryDto> Handle(
            GetOrdersDashboardChartsQuery request,
            CancellationToken cancellationToken)
        {
            var revenueStatuses = new[] { "Approved", "InProgress", "Completed" };

            var orders = await context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Year == request.Year)
                .Select(o => new
                {
                    o.OrderDate,
                    o.Status,
                    o.TotalAmount
                })
                .ToListAsync(cancellationToken);

            var ordersByMonth = new int[12];
            var revenueByMonth = new decimal[12];

            foreach (var order in orders)
            {
                var monthIndex = order.OrderDate.Month - 1;

                ordersByMonth[monthIndex]++;

                if (revenueStatuses.Contains(order.Status))
                {
                    revenueByMonth[monthIndex] += order.TotalAmount;
                }
            }

            return new GetOrdersDashboardChartsQueryDto
            {
                OrdersByMonth = ordersByMonth,
                RevenueByMonth = revenueByMonth
            };
        }
    }
}
