using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IUserFeedItemRepository
{
    Task<UserFeedItem?> GetAsync(int userId, int feedItemId);
    Task<UserFeedItem> CreateAsync(int userId, int feedItemId);
    Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true);
    Task MarkAsFavoriteAsync(int userId, int feedItemId, bool isFavorite);
}
