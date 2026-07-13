using RssReader.DTOs.FeedItem;

namespace RssReader.Services.Interfaces;

public interface IFeedItemService
{
    Task<FeedItemGroupedDto> GetFeedItemsGroupedAsync(int userId, int feedId, int pageNumber = 1, int pageSize = 50, CancellationToken ct = default);
    Task<List<FeedItemDto>> GetFeedItemFilteredAsync(int userId, FeedItemFilterQuery filter, CancellationToken ct = default);
    Task<FeedItemDto> GetFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default);
    Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true, CancellationToken ct = default);
    Task ChangeFavoriteStatusAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default);
    Task RemoveFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default);
}
