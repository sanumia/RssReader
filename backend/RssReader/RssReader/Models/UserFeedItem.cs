namespace RssReader.Models;

public class UserFeedItem
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int FeedItemId { get; set; }
    public FeedItem FeedItem { get; set; } = null!;

    public bool IsRead { get; set; } = false;
}
