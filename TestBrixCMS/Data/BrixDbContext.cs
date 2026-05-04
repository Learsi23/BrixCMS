using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestBrixCMS.Data;

public class BrixDbContext : DbContext
{
    public BrixDbContext(DbContextOptions<BrixDbContext> options) : base(options) { }

    public DbSet<Page> Pages { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<SiteConfig> SiteConfig { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<PageView> PageViews { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiKey>()
            .HasIndex(k => k.Provider)
            .IsUnique();
    }
}

public class Page
{
    public Guid Id { get; set; }
    [Required] public string Title { get; set; }
    public string? Slug { get; set; }
    public string? JsonData { get; set; }
    public List<Block>? Blocks { get; set; } = new();
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsSeed { get; set; } = false;

    public string? PageType { get; set; } = "standard";
    public string? MetaDescription { get; set; }
    public string? OgImage { get; set; }
    public string? MetaKeywords { get; set; }
}
public class Block
{
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public int SortOrder { get; set; }

    public string? JsonData { get; set; }

    public Guid PageId { get; set; }

    public Guid? ParentId { get; set; }
}

public class SiteConfig
{
    public int Id { get; set; }   // 0 = auto-increment; do not set to 1 to allow multiple entries
    public string Key { get; set; } = "site";
    public string? JsonData { get; set; }
}

public class AdminUser
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    public bool IsOwner { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PageView
{
    public int Id { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    public string Slug { get; set; } = "";
    public string? UserAgent { get; set; }
    public string? Referrer { get; set; }
}

public class Subscriber
{
    public int Id { get; set; }
    [Required, EmailAddress] public string Email { get; set; } = "";
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

