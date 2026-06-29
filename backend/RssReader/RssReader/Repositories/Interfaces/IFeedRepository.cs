using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IFeedRepository : IBaseRepository<Feed>
{
    Task<List<Feed>> GetAllByUserIdAsync(int id);
    Task<Feed?> GetByUrlAsync(string url);
    Task<bool> ExistsByUrlAsync(string url);

    Task<bool> UserIsSubscribedAsync(int userId, int feedId);
    Task SubscribeUserToFeedAsync(int userId, int feedId);
    Task UnsubscribeUserFromFeedAsync(int userId, int feedId);

}
