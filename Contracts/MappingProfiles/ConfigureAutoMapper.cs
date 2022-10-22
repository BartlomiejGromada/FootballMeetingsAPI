using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.MappingProfiles;

public static class ConfigureAutoMapper
{
    public static void AddAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(cfg =>
        {
            cfg.ValueTransformers.Add<byte[]>(value => value.Length == 0 ? null : value);
        }, System.Reflection.Assembly.GetExecutingAssembly());
    }
}
