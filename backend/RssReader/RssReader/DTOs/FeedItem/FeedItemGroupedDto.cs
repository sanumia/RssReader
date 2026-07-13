namespace RssReader.DTOs.FeedItem;

public class FeedItemGroupedDto
{
    public List<FeedItemDto> Today { get; set; } = new();
    public List<FeedItemDto> Yesterday { get; set; } = new();
    public List<FeedItemDto> LastWeek { get; set; } = new();
    public List<FeedItemDto> Older { get; set; } = new();

}
