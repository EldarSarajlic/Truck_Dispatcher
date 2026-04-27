using Dispatcher.Domain.Entities.Identity;

namespace Dispatcher.Application.Modules.Users.Commands.Update;

public class UpdateUserCommand : IRequest<Unit>
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public required UserRole Role { get; set; }
    public required bool IsEnabled { get; set; }
    public int? CityId { get; set; }
}
