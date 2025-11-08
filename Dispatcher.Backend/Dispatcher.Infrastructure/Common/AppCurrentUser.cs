using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Dispatcher.Application.Abstractions;

namespace Dispatcher.Infrastructure.Common;

/// <summary>
/// Implementation of IAppCurrentUser that reads data from a JWT token.
/// </summary>
public sealed class AppCurrentUser(IHttpContextAccessor httpContextAccessor)
    : IAppCurrentUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? UserId =>
        int.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : null;

    public string? Email =>
        _user?.FindFirstValue(ClaimTypes.Email);

    public string? FirstName =>
        _user?.FindFirstValue(ClaimTypes.GivenName);

    public string? LastName =>
        _user?.FindFirstValue(ClaimTypes.Surname);

    public string? DisplayName =>
        _user?.FindFirstValue("display_name");

    public string? PhoneNumber =>
        _user?.FindFirstValue(ClaimTypes.MobilePhone);

    public bool IsAuthenticated =>
        _user?.Identity?.IsAuthenticated ?? false;

    public UserRole? Role =>
    Enum.TryParse<UserRole>(_user?.FindFirstValue(ClaimTypes.Role), ignoreCase: true, out var role)
        ? role
        : null;
}