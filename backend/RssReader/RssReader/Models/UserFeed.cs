namespace RssReader.Models;

public class UserFeed
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int FeedId { get; set; }
    public Feed Feed { get; set; } = null!;
}
