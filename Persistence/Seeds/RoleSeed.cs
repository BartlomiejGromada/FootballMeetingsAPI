using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seeds;

public static class RoleSeed
{
    public static void SeedData(FootballMeetingsDbContext dbContext)
    {
        if(!dbContext.Roles.Any())
        {
            var admin = new Role()
            {
                Id = 1,
                Name = "Admin",
            };

            var creator = new Role()
            {
                Id = 2,
                Name = "Creator",
            };

            var user = new Role()
            {
                Id = 3,
                Name = "User",
            };

            using var transaction = dbContext.Database.BeginTransaction();

            dbContext.Roles.AddRange(admin, creator, user);
            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Roles ON;");
            dbContext.SaveChanges();

            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Roles OFF;");
            transaction.Commit();
        }
    }
}
