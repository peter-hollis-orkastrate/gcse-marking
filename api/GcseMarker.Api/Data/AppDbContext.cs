using Microsoft.EntityFrameworkCore;
using GcseMarker.Api.Models.Entities;

namespace GcseMarker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UsageLog> UsageLogs => Set<UsageLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(255);
            entity.Property(e => e.Enabled).HasColumnName("Enabled").HasDefaultValue(true);
            entity.Property(e => e.IsAdmin).HasColumnName("IsAdmin").HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime2");
            entity.Property(e => e.LastLoginAt).HasColumnName("LastLoginAt").HasColumnType("datetime2");
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<UsageLog>(entity =>
        {
            entity.ToTable("UsageLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.SkillUsed).HasColumnName("SkillUsed").HasMaxLength(100).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime2");
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UsageLogs_UserId");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_UsageLogs_CreatedAt").IsDescending();
            entity.HasIndex(e => e.SkillUsed).HasDatabaseName("IX_UsageLogs_SkillUsed");
            entity.HasOne(e => e.User)
                  .WithMany(u => u.UsageLogs)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
