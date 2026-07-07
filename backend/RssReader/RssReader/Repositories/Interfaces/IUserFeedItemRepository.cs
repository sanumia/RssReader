using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IUserFeedItemRepository
{
    Task<UserFeedItem?> GetAsync(int userId, int feedItemId, CancellationToken ct = default);
    Task<UserFeedItem> CreateAsync(int userId, int feedItemId, CancellationToken ct = default);
    Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true, CancellationToken ct = default);
    Task MarkAsFavoriteAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default);
    Task MarkAsRemovedAsync(int userId, int feedItemId, bool IsRemoved = true, CancellationToken ct = default);
}
