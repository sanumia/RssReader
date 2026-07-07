namespace RssReader.DTOs.Feed;

public class DashboardFeedDto
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string? Title { get; set; }
    public string? IconUrl { get; set; }
    public List<string> FolderNames { get; set; } = new();
    public int FeedItemCount { get; set; }
}
