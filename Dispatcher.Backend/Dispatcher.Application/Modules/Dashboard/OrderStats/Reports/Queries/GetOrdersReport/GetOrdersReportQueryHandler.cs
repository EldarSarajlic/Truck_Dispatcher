using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Reports.Queries.GetOrdersReport
{
    public class GetOrdersReportQueryHandler(IAppDbContext context)
        : IRequestHandler<GetOrdersReportQuery, GetOrdersReportQueryDto>
    {
        public async Task<GetOrdersReportQueryDto> Handle(
            GetOrdersReportQuery request,
            CancellationToken ct)
        {
            var revenueStatuses = new[] { "Approved", "InProgress", "Completed" };

            var ordersQuery = context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Year == request.Year);

            if (request.Month.HasValue)
            {
                ordersQuery = ordersQuery
                    .Where(o => o.OrderDate.Month == request.Month.Value);
            }

            var orders = await ordersQuery.ToListAsync(ct);
            var totalOrders = orders.Count;

            var revenueOrders = orders
                .Where(o => revenueStatuses.Contains(o.Status))
                .ToList();

            var cancelledOrderIds = orders
    .Where(o => o.Status == "Cancelled")
    .Select(o => o.Id)
    .ToHashSet();
            // ===== ITEMS =====
            var orderIds = orders.Select(o => o.Id).ToList();

            var orderItems = await context.OrderItems
                .AsNoTracking()
                .Include(i => i.Inventory)
                .Where(i => orderIds.Contains(i.OrderId))
                .ToListAsync(ct);

            return new GetOrdersReportQueryDto
            {
                PeriodLabel = request.Month.HasValue
                    ? $"{request.Month}/{request.Year}"
                    : $"Year {request.Year}",

                // ===== FINANCE =====
                TotalRevenue = revenueOrders.Sum(o => o.TotalAmount),
                AvgOrderValue = revenueOrders.Any() ? revenueOrders.Average(o => o.TotalAmount) : 0,
                MaxOrderValue = revenueOrders.Any() ? revenueOrders.Max(o => o.TotalAmount) : 0,
                MinOrderValue = revenueOrders.Any() ? revenueOrders.Min(o => o.TotalAmount) : 0,

                RevenueByCurrency = revenueOrders
                    .GroupBy(o => o.Currency)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalAmount)),

                // ===== FLOW =====
                TotalOrders = totalOrders,
                ApprovalRate = totalOrders > 0
                    ? (decimal)orders.Count(o => o.Status == "Approved") / totalOrders * 100
                    : 0,

                CancelRate = totalOrders > 0
                    ? (decimal)orders.Count(o => o.Status == "Cancelled") / totalOrders * 100
                    : 0,

                AvgDeliveryTimeDays = orders
                    .Where(o => o.RequestedDeliveryDate != null)
                    .Select(o => (o.RequestedDeliveryDate.Value - o.OrderDate).TotalDays)
                    .DefaultIfEmpty()
                    .Average(),

                PriorityStats = orders
                    .GroupBy(o => o.Priority)
                    .ToDictionary(g => g.Key, g => g.Count()),

                // ===== ITEMS =====
                AvgItemsPerOrder = orderItems
                    .GroupBy(i => i.OrderId)
                    .Select(g => g.Sum(x => x.Quantity))
                    .DefaultIfEmpty()
                    .Average(),

                TopSellingItems = orderItems
                    .GroupBy(i => i.Inventory)
                    .Select(g => new ReportItemDto
                    {
                        InventoryId = g.Key.Id,
                        Name = g.Key.Name,
                        Quantity = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.TotalPrice)
                    })
                    .OrderByDescending(x => x.Quantity)
                    .Take(5)
                    .ToList(),

                TopProfitableItems = orderItems
                    .GroupBy(i => i.Inventory)
                    .Select(g => new ReportItemDto
                    {
                        InventoryId = g.Key.Id,
                        Name = g.Key.Name,
                        Quantity = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.TotalPrice)
                    })
                    .OrderByDescending(x => x.Revenue)
                    .Take(5)
                    .ToList(),

                MostCancelledItems = orderItems
    .Where(i => cancelledOrderIds.Contains(i.OrderId))
    .GroupBy(i => new { i.InventoryId, i.Inventory.Name })
    .Select(g => new ReportItemDto
    {
        InventoryId = g.Key.InventoryId,
        Name = g.Key.Name,
        Quantity = g.Sum(x => x.Quantity),
        Revenue = 0
    })
    .OrderByDescending(x => x.Quantity)
    .Take(5)
    .ToList()

            };
        }
    }
}
