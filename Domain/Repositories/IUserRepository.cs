using Domain.Entities;

namespace Domain.Repositories;

public interface IUsersRepository
{
    Task<bool> ExistsByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(int userId, bool isActive = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsDeletedUser(int userId, CancellationToken cancellationToken = default);
}