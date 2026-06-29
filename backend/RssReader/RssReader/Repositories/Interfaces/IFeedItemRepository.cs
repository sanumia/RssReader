using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IFeedItemRepository : IBaseRepository<FeedItem>
{
    Task<List<FeedItem>> GetByFeedIdAsync(int feedId, int skip, int take);
    Task<List<FeedItem>> GetAllForUserAsync(int userId, bool? isRead, bool? isFavorite, DateTime? from, DateTime? to);
    Task AddRangeAsync(List<FeedItem> feedItems);
}
