using System.Security.Claims;

namespace Services.Abstractions;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    int GetUserId { get; }
    string GetUserRole { get; }
}
