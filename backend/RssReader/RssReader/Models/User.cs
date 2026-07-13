namespace RssReader.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<UserFeed> UserFeeds { get; set; } = new List<UserFeed>();
    public ICollection<UserFeedItem> UserFeedItems { get; set; } = new List<UserFeedItem>();
}
