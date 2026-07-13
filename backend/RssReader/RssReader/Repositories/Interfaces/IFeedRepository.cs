using RssReader.DTOs.Feed;
using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IFeedRepository : IBaseRepository<Feed>
{
    Task<List<Feed>> GetAllByUserIdAsync(int id, CancellationToken ct = default);
    Task<Feed?> GetByUrlAsync(string url, CancellationToken ct = default);
    Task<bool> ExistsByUrlAsync(string url, CancellationToken ct = default);

    Task<bool> UserIsSubscribedAsync(int userId, int feedId, CancellationToken ct = default);
    Task SubscribeUserToFeedAsync(int userId, int feedId, CancellationToken ct = default);
    Task UnsubscribeUserFromFeedAsync(int userId, int feedId, CancellationToken ct = default);
    Task<List<DashboardFeedDto>> GetDashboardFeedsAsync(int userId, CancellationToken ct = default);

}
