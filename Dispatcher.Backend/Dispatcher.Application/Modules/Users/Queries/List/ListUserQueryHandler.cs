using Dispatcher.Application.Modules.Users.Queries.List;

namespace Dispatcher.Application.Modules.Queries.List;

public sealed class ListUserQueryHandler(IAppDbContext ctx): IRequestHandler<ListUserQuery,PageResult<ListUserQueryDto>>
{
    public async Task<PageResult<ListUserQueryDto>> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        var query = ctx.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(u => u.FirstName.ToLower().Contains(search) ||
                                     u.LastName.ToLower().Contains(search) ||
                                     (u.FirstName + " " + u.LastName).ToLower().Contains(search) ||
                                     u.Email.ToLower().Contains(search));
        }

        if (request.OnlyEnabled is not null)
        {
            query = query.Where(u => u.IsEnabled == request.OnlyEnabled.Value);
        }

    var projectedQuery = query
    .OrderBy(u => u.FirstName)
    .ThenBy(u => u.LastName)
    .Select(u => new ListUserQueryDto
    {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        DisplayName = u.DisplayName,
        Email = u.Email,
        PhoneNumber = u.PhoneNumber,
        DateOfBirth = u.DateOfBirth,
        Role = u.Role.ToString(),
        IsEnabled = u.IsEnabled,
        CityId = u.CityId,
        CityName = u.City != null ? u.City.Name : null,
        ProfilePhotoUrl = u.ProfilePhotoUrl
    });
        return await PageResult<ListUserQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
    }
}