namespace Web.Extensions;

public static class ConfigureCorsExtension
{
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
