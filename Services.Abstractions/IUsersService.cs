namespace Services.Abstractions;

public interface IUsersService
{
    Task RemoveUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
