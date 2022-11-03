using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contracts;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddFluentValidationClientsideAdapters();
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return serviceCollection;
    }
}
