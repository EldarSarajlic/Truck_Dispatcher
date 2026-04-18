namespace Dispatcher.Application.Common;

public sealed class PageResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public bool IncludedTotal { get; init; }

    /// <summary>
    /// Creates a PageResult from an IQueryable using EF Core asynchronous methods.
    /// </summary>
    public static async Task<PageResult<T>> FromQueryableAsync(
        IQueryable<T> query,
        PageRequest paging,
        CancellationToken ct = default,
        bool includeTotal = true)
    {
        int total = 0;
        if (includeTotal)
            total = await query.CountAsync(ct);

        var items = await query
            .Skip(paging.SkipCount)
            .Take(paging.PageSize)
            .ToListAsync(ct);

        var totalPages = paging.PageSize > 0 && total > 0
            ? (int)Math.Ceiling((double)total / paging.PageSize)
            : 1;

        return new PageResult<T>
        {
            Items        = items,
            TotalItems   = total,
            TotalPages   = totalPages,
            CurrentPage  = paging.Page,
            PageSize     = paging.PageSize,
            IncludedTotal = includeTotal,
        };
    }
}