 namespace RssReader.Models;

public class Feed
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsActive { get; set; } = true;
    public string? IconUrl { get; set; }

    public ICollection<FeedItem> FeedItems { get; set; } = new List<FeedItem>();
    public ICollection<UserFeed> UserFeeds { get; set; } = new List<UserFeed>();
    public ICollection<FeedFolder> FeedFolders { get; set; } = new List<FeedFolder>();
}
