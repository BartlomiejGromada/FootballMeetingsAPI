using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistence;

public sealed class FootballMeetingsDbContext : DbContext
{
    public FootballMeetingsDbContext(DbContextOptions<FootballMeetingsDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FootballMatch> FootballMatches { get; set; }
    public DbSet<FootballPitch> FootballPitches { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FootballMeetingsDbContext).Assembly);
    }
}

public static class Extensions
{
    public static void AddDbContext(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<FootballMeetingsDbContext>(option =>
        {
            option.UseSqlServer(connectionString)
                  .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }
}