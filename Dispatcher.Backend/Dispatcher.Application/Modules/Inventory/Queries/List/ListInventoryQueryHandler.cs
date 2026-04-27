using Dispatcher.Application.Modules.Inventory.Queries.List;

namespace Dispatcher.Application.Modules.Inventory.Queries.List;

public sealed class ListInventoryQueryHandler(IAppDbContext ctx)
    : IRequestHandler<ListInventoryQuery, PageResult<ListInventoryQueryDto>>
{
    public async Task<PageResult<ListInventoryQueryDto>> Handle(
        ListInventoryQuery request, CancellationToken cancellationToken)
    {
        var query = ctx.Inventory
            .Include(i => i.Photo)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim().ToLower();
            query = query.Where(i =>
                i.Name.ToLower().Contains(s) ||
                i.SKU.ToLower().Contains(s) ||
                (i.Description != null && i.Description.ToLower().Contains(s)));
        }

        if (!string.IsNullOrWhiteSpace(request.Category))
            query = query.Where(i => i.Category == request.Category);

        var projected = query
            .OrderBy(i => i.Category)
            .ThenBy(i => i.Name)
            .Select(i => new ListInventoryQueryDto
            {
                Id            = i.Id,
                SKU           = i.SKU,
                Name          = i.Name,
                Description   = i.Description,
                Category      = i.Category,
                UnitOfMeasure = i.UnitOfMeasure,
                UnitPrice     = i.UnitPrice,
                UnitWeight    = i.UnitWeight,
                UnitVolume    = i.UnitVolume,
                IsActive      = i.IsActive,
                PhotoUrl      = i.Photo != null ? i.Photo.Url          : null,
                ThumbnailUrl  = i.Photo != null ? i.Photo.ThumbnailUrl : null,
            });

        return await PageResult<ListInventoryQueryDto>.FromQueryableAsync(
            projected, request.Paging, cancellationToken);
    }
}
