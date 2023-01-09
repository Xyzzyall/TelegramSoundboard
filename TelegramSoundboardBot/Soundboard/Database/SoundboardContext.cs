using Microsoft.EntityFrameworkCore;
using TelegramSoundboardBot.Soundboard.Database.Models;

namespace TelegramSoundboardBot.Soundboard.Database;

public class SoundboardContext : DbContext
{
    public SoundboardContext(DbContextOptions<SoundboardContext> options) : base(options)
    {
    }

    public DbSet<Sound> Sounds { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sound>().ToTable("Sounds");
        modelBuilder.Entity<Sound>().Property(s => s.Name).UseCollation("NOCASE");
        modelBuilder.Entity<Sound>().HasIndex(s => s.Name).IsUnique();
    }
}