using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FootballPitchConfiguration : IEntityTypeConfiguration<FootballPitch>
{
    public void Configure(EntityTypeBuilder<FootballPitch> builder)
    {
        builder.Property(fp => fp.Name).IsRequired();
        builder.Property(fp => fp.City).IsRequired();
    }
}
