using Contracts.Models.Account;
using Microsoft.AspNetCore.JsonPatch;

namespace Services.Abstractions;

public interface IAccountService
{
    Task<int> RegisterUser(RegisterUserDto dto);
    Task<string> GenerateJwt(LoginUserDto dto);
    Task RemoveUserById(int userId, string password);
    Task RestoreUserById(int userId);
    Task UpdateAccountPatch(int userId, JsonPatchDocument user);
}
