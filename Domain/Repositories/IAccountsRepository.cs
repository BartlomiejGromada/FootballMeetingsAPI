using Domain.Entities;

namespace Domain.Repositories;

public interface IAccountsRepository
{
    Task<int> RegisterUser(User user);
    Task RemoveUserById(int userId);
    Task RestoreUserById(int userId);
}
