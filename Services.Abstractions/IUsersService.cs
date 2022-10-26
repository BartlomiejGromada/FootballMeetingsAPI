using Contracts.Models.User;

namespace Services.Abstractions;

public interface IUsersService
{
    Task<UserDto> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
}
