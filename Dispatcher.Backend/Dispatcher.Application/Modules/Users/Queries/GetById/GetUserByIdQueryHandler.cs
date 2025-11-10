

using Dispatcher.Application.Modules.Users.Queries.GetById;
using Microsoft.Extensions.DependencyInjection;

namespace Dispatcher.Application.Modules.Users.Queries.GetById;

public class GetUserByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetUserByIdQuery,GetUserByIdQueryDto>
{

    public async Task<GetUserByIdQueryDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(u => u.Id == request.Id && !u.IsDeleted)
            .Select(u => new GetUserByIdQueryDto
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
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new MarketNotFoundException($"User with ID {request.Id} not found.");

        return user;
    }
}