using Sieve.Models;
using Sieve.Services;
using Web.Sieve;

namespace Web.Extensions;

public static class SieveExtension
{
    public static IServiceCollection AddSieve(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
        serviceCollection.Configure<SieveOptions>(configuration);

        return serviceCollection;
    }
}
