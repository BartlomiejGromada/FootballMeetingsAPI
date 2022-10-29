using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.Content).IsRequired();
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId);

        builder.HasOne(c => c.FootballMatch)
            .WithMany(fm => fm.Comments)
            .HasForeignKey(c => c.FootballMatchId);
    }
}
