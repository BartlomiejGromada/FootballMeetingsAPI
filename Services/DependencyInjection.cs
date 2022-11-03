using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Services.Services;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplications(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<IFootballPitchesService, FootballPitchesService>();
        serviceCollection.AddScoped<IFootballMatchesService, FootballMatchesService>();
        serviceCollection.AddScoped<IUsersService, UsersService>();
        serviceCollection.AddScoped<IFootballMatchesPlayersService, FootballMatchesPlayersService>();
        serviceCollection.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        serviceCollection.AddScoped<IUserContextService, UserContextService>();
        serviceCollection.AddScoped<ICommentsService, CommentsService>();

        return serviceCollection;
    }
}
