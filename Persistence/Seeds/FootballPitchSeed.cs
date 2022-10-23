using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seeds;

public static class FootballPitchSeed
{
    public static void SeedData(FootballMeetingsDbContext dbContext)
    {
        if (!dbContext.FootballPitches.Any())
        {
            var footballPitchOne = new FootballPitch()
            {
                Id = 1,
                Name = "Orlik Zębców",
                City = "Ostrów Wielkopolski",
                Street = "Zębcowska",
                StreetNumber = "18",
                Image = null,
            };

            var footballPitchTwo = new FootballPitch()
            {
                Id = 2,
                Name = "Boisko Sokołów Droszew",
                City = "Droszew",
                Street = "Kościelna",
                StreetNumber = "2",
                Image = null,
            };

            using var transaction = dbContext.Database.BeginTransaction();

            dbContext.FootballPitches.AddRange(footballPitchOne, footballPitchTwo);
            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FootballPitches ON;");
            dbContext.SaveChanges();

            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.FootballPitches OFF;");
            transaction.Commit();
        }
    }
}