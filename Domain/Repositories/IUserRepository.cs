using Domain.Entities;

namespace Domain.Repositories;

public interface IUsersRepository
{
    Task RegisterUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task RemoveUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}