using RssReader.Constants;

namespace RssReader.DTOs.FeedItem;

public class GlobalFeedItemsFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int PageNumber { get; set; } = PaginationConstants.DefaultPageNumber;
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
}
