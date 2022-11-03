namespace Web.Extensions;

public static class ApiVersionExtension
{
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
}
