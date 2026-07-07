namespace RssReader.DTOs.FeedItem;

public class FeedItemFilterQuery
{
    public bool? IsRead { get; set; }
    public bool? IsFavorite { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
