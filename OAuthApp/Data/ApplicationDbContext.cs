using Microsoft.EntityFrameworkCore;
using OAuthApp.Models;

namespace OAuthApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Reality> Realities { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Picture> Pictures { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>()
            .HasOne(ch => ch.Reality)
            .WithMany(r => r.Characters)
            .HasForeignKey(ch => ch.RealityId);

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Reality)
            .WithMany(r => r.Locations)
            .HasForeignKey(l => l.RealityId);

    }






}
