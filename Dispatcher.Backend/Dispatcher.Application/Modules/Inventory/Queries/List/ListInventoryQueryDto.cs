namespace Dispatcher.Application.Modules.Inventory.Queries.List;

public sealed class ListInventoryQueryDto
{
    public int     Id            { get; init; }
    public string  SKU           { get; init; } = string.Empty;
    public string  Name          { get; init; } = string.Empty;
    public string? Description   { get; init; }
    public string  Category      { get; init; } = string.Empty;
    public string  UnitOfMeasure { get; init; } = string.Empty;
    public decimal UnitPrice     { get; init; }
    public decimal? UnitWeight   { get; init; }
    public decimal? UnitVolume   { get; init; }
    public bool    IsActive      { get; init; }
    public string? PhotoUrl      { get; init; }
    public string? ThumbnailUrl  { get; init; }
}
