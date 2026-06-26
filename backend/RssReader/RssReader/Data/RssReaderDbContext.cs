using Microsoft.EntityFrameworkCore;
using RssReader.Models;

namespace RssReader.Data;

public class RssReaderDbContext(DbContextOptions<RssReaderDbContext> options) : DbContext(options)
{
    public DbSet<Feed> Feeds => Set<Feed>();
    public DbSet<FeedItem> FeedItems => Set<FeedItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserFeed> UserFeeds => Set<UserFeed>();
    public DbSet<UserFeedItem> UserFeedItems => Set<UserFeedItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedItem>()
            .HasOne(fi => fi.Feed)
            .WithMany(f => f.FeedItems)
            .HasForeignKey(fi => fi.FeedId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserFeed>()
            .HasKey(uf => new { uf.UserId, uf.FeedId });

        modelBuilder.Entity<UserFeed>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UserFeeds)
            .HasForeignKey(uf => uf.UserId);

        modelBuilder.Entity<UserFeed>()
            .HasOne(uf => uf.Feed)
            .WithMany(f => f.UserFeeds)
            .HasForeignKey(uf => uf.FeedId);

        modelBuilder.Entity<UserFeedItem>()
            .HasKey(ufi => new { ufi.UserId, ufi.FeedItemId });

        modelBuilder.Entity<UserFeedItem>()
            .HasOne(ufi => ufi.User)
            .WithMany(u => u.UserFeedItems)
            .HasForeignKey(ufi => ufi.UserId);

        modelBuilder.Entity<UserFeedItem>()
            .HasOne(ufi => ufi.FeedItem)
            .WithMany(fi => fi.UserFeedItems)
            .HasForeignKey(ufi => ufi.FeedItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feed>()
            .HasIndex(f => f.Url)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
