using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Users.Commands.Update;

public class UpdateUserCommandHandler(IAppDbContext context) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);

        if (user is null)
            throw new MarketNotFoundException($"User with ID {request.Id} not found.");

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.DisplayName = $"{request.FirstName.Trim()} {request.LastName.Trim()}";
        user.NormalizedDisplayName = user.DisplayName.ToUpperInvariant();
        user.Email = request.Email.Trim();
        user.PhoneNumber = request.PhoneNumber?.Trim();
        user.DateOfBirth = request.DateOfBirth;
        user.Role = request.Role;
        user.IsEnabled = request.IsEnabled;
        user.CityId = request.CityId;

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
