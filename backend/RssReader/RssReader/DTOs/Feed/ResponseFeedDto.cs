namespace RssReader.DTOs.Feed;

public class ResponseFeedDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? IconUrl { get; set; }
}
