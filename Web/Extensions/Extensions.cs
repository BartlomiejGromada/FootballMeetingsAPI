using Persistence;
using Persistence.Seeds;

namespace Web.Extensions;

public static class Extensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "Football Meetings API",
                Description = "An ASP.NET Core Web API for organizing and managing football meetings",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Send email to me :)",
                    Email = "bartlomiejgromada97@gmail.com",
                },
            });
        });

        return serviceCollection;
    }

    public static IServiceCollection ConfigureApiVersion(this IServiceCollection serviceCollection)
    {
       serviceCollection.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        return serviceCollection;
    }

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
