using RssReader.DTOs.FeedItem;
using RssReader.Constants;

namespace RssReader.Services.Interfaces;

public interface IFeedItemService
{
    Task<List<FeedItemDto>> GetAllFeedItemsFilteredAsync(int userId, FeedItemFilterQuery feedItemFilterQuery, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<FeedItemGroupedDto> GetAllFeedItemsGroupedAsync(int userId, int feedId, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<FeedItemGroupedDto> GetFeedItemsGroupedAsync(int userId, int feedId, int pageNumber = 1, int pageSize = PaginationConstants.DefaultPageSize, CancellationToken ct = default);
    Task<List<FeedItemDto>> GetFeedItemFilteredAsync(int userId, FeedItemFilterQuery filter, CancellationToken ct = default);
    Task<FeedItemDto> GetFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default);
    Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true, CancellationToken ct = default);
    Task ChangeFavoriteStatusAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default);
    Task RemoveFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default);
}
