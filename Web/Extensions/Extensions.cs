using Contracts.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    Array.Empty<string>()
                }
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

        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

    public static IServiceCollection ConfigureCors(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy("FrontendClient", builder =>
            {
                builder
                .AllowAnyOrigin() //allow any methods 
                .AllowAnyHeader() //allow any header
                .WithOrigins(configuration["AllowedOrigins"]); //origin which can send request to api
            });
        });

        return serviceCollection;
    }
}
