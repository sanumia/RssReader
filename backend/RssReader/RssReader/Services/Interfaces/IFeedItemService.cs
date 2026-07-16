using RssReader.DTOs.FeedItem;
using RssReader.Constants;

namespace RssReader.Services.Interfaces;

public interface IFeedItemService
{
    Task<List<FeedItemDto>> GetGlobalFeedItemsAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<List<FeedItemDto>> GetPersonalFeedItemsAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<List<FeedItemDto>> GetPersonalFeedItemsFilteredAsync(bool? isRead, bool? isFavorite, DateTime? from, DateTime? to, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<List<FeedItemDto>> GetFeedItemsByFeedIdAsync(int feedId, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<FeedItemGroupedDto> GetFeedItemsGroupedByFeedIdAsync(int feedId, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<FeedItemDto> GetFeedItemAsync(int feedItemId, CancellationToken ct = default);
    Task MarkAsReadAsync(int feedItemId, bool isRead = true, CancellationToken ct = default);
    Task ChangeFavoriteStatusAsync(int feedItemId, bool isFavorite, CancellationToken ct = default);
    Task RemoveFeedItemAsync(int feedItemId, CancellationToken ct = default);
}
