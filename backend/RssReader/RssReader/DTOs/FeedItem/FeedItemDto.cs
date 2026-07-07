namespace RssReader.DTOs.FeedItem;

public class FeedItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Link { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public string? IconUrl { get; set; }
    public string? Attachments { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsFavorite { get; set; } = false;
}
