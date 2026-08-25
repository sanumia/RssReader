using System.ComponentModel.DataAnnotations;

namespace RssReader.DTOs.FeedItem;

public class UpdateFeedItemDto
{
    [StringLength(200)]
    public string? Title { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? Link { get; set; }
    public string? IconUrl { get; set; }
}
