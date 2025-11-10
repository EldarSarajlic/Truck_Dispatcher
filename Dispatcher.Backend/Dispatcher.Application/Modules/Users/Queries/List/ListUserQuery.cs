namespace Dispatcher.Application.Modules.Users.Queries.List;

public sealed class ListUserQuery : BasePagedQuery<ListUserQueryDto>
{
    public string? Search { get; init; }
    public bool? OnlyEnabled { get; init; }
}