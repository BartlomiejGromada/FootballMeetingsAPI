using Microsoft.AspNetCore.Http;
using Services.Abstractions;
using System.Security.Claims;

namespace Services;

public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserContextService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    public int GetUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) != null ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : 0;

    public string GetUserRole => User.FindFirstValue(ClaimTypes.Role) ?? "";
}
