using Persistence;
using Persistence.Seeds;

namespace Web.Extensions;

public static class SeedDataExtension
{
    public static WebApplication SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<FootballMeetingsDbContext>();
        RoleSeed.SeedData(dbContext);
        FootballPitchSeed.SeedData(dbContext);
        UserSeed.SeedData(dbContext);

        return webApplication;
    }
}
