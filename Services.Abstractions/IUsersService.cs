using Contracts.Models.User;

namespace Services.Abstractions;

public interface IUsersService
{
    Task<int> RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
    Task RemoveUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
