using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.NickName).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(150);
        builder.Property(u => u.LastName).HasMaxLength(200);

        builder.HasQueryFilter(u => u.IsActive);

        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        builder.HasMany(u => u.FootballMatches)
            .WithMany(fm => fm.Players)
            .UsingEntity<FootballMatchPlayer>
            (
                fmp => fmp.HasOne(fmp => fmp.FootballMatch)
                .WithMany()
                .HasForeignKey(fmp => fmp.FootballMatchId),

                fmp => fmp.HasOne(fmp => fmp.Player)
                .WithMany()
                .HasForeignKey(fmp => fmp.PlayerId),

                fmp =>
                {
                    fmp.ToTable("FootballMatchesPlayers");
                    fmp.HasKey(builder => new { builder.FootballMatchId, builder.PlayerId });
                    fmp.Property(builder => builder.JoiningDate).HasDefaultValueSql("GETDATE()");
                }
            );
    }
}
