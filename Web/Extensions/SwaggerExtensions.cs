using Microsoft.OpenApi.Models;

namespace Web.Extensions;

public static class SwaggerExtensions
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
}
