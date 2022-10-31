using Domain.Entities;
using Sieve.Services;

namespace Web.Sieve;

public class SieveConfigurationForFootballPitch : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<FootballPitch>(fp => fp.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<FootballPitch>(fp => fp.City)
            .CanFilter()
            .CanSort();
    }
}
