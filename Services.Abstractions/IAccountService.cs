using Contracts.Models.Account;

namespace Services.Abstractions;

public interface IAccountService
{
    Task<int> RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
    Task<string> GenerateJwtAsync(LoginUserDto dto, CancellationToken cancellationToken = default);
}
