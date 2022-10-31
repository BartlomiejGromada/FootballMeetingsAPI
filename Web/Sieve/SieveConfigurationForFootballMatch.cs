using Domain.Entities;
using Sieve.Services;

namespace Web.Sieve;

public class SieveConfigurationForFootballMatch : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<FootballMatch>(fm => fm.Date)
            .CanFilter()
            .CanSort();

        mapper.Property<FootballMatch>(fm => fm.Creator.NickName)
            .CanFilter()
            .HasName("creator");

        mapper.Property<FootballMatch>(fm => fm.CreatedAt)
            .CanFilter()
            .CanSort();

        mapper.Property<FootballMatch>(fm => fm.MaxNumberOfPlayers)
            .CanFilter();

        mapper.Property<FootballMatch>(fm => fm.FootballPitch.Name)
            .CanFilter()
            .CanSort()
            .HasName("footballPitch");
    }
}
