using Contracts.Models.Account;

namespace Services.Abstractions;

public interface IAccountService
{
    Task<int> RegisterUser(RegisterUserDto dto);
    Task<string> GenerateJwt(LoginUserDto dto);
    Task RemoveUserById(int userId, string password);
    Task RestoreUserById(int userId);
}
