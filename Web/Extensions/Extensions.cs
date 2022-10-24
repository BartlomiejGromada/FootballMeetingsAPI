using Contracts.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Seeds;
using System.Text;

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

    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var authenticationSettings = new AuthenticationSettings();
        configuration.GetSection("Authentication").Bind(authenticationSettings);
        serviceCollection.AddSingleton(authenticationSettings);

        serviceCollection.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = authenticationSettings.JwtIssuer,
                ValidAudience = authenticationSettings.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
            };
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
