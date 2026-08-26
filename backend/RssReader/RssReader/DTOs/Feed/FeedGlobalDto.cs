namespace RssReader.DTOs.Feed;

public class FeedGlobalDto
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int FeedItemCount { get; set; }    
    public bool IsSubscribed { get; set; }         
    public List<string> FolderNames { get; set; } = new();
}
