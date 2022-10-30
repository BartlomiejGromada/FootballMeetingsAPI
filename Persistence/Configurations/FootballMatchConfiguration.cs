using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FootballMatchConfiguration : IEntityTypeConfiguration<FootballMatch>
{
    public void Configure(EntityTypeBuilder<FootballMatch> builder)
    {
        builder.Property(fm => fm.Name).IsRequired();
        builder.Property(fm => fm.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.HasQueryFilter(fm => fm.IsActive);

        builder.HasOne(fm => fm.FootballPitch)
            .WithMany(fp => fp.FootballMatches)
            .HasForeignKey(fm => fm.FootballPitchId);

        builder.HasOne(fm => fm.Creator)
            .WithMany()
            .HasForeignKey(fm => fm.CreatorId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(fm => fm.Comments)
            .WithOne(c => c.FootballMatch)
            .HasForeignKey(c => c.FootballMatchId);
    }
}
