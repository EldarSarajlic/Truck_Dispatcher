namespace Dispatcher.Application.Modules.Inventory.Queries.List;

public sealed class ListInventoryQuery : BasePagedQuery<ListInventoryQueryDto>
{
    public string? Search   { get; init; }
    public string? Category { get; init; }
}
