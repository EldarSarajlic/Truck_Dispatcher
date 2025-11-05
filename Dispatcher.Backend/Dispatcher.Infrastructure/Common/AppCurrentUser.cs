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

    public bool IsAuthenticated =>
        _user?.Identity?.IsAuthenticated ?? false;

    private string? UserRole => _user?.FindFirstValue(ClaimTypes.Role);
    public bool IsAdmin => UserRole == "Admin";
    public bool IsDispatcher => UserRole == "Dispatcher";
    public bool IsDriver => UserRole == "Driver";
    public bool IsClient => UserRole == "Client";
}