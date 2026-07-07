using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FeedRepository(RssReaderDbContext context) : BaseRepository<Feed>(context), IFeedRepository
{
    public async Task<bool> ExistsByUrlAsync(string url, CancellationToken ct = default)
    {
        return await context.Feeds.AnyAsync(f => f.Url == url, ct);
    }

    public async Task<List<Feed>> GetAllByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await context.UserFeeds
            .Where(uf => uf.UserId == userId)
            .Include(uf => uf.Feed)
            .ThenInclude(f => f.FeedItems)
            .Select(uf => uf.Feed)
            .ToListAsync(ct);
    }

    public async Task<Feed?> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        return await context.Feeds.FirstOrDefaultAsync(f => f.Url == url, ct);
    }

    public async Task SubscribeUserToFeedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        var alreadySubscribed = await UserIsSubscribedAsync(userId, feedId);

        if (alreadySubscribed) 
            return;

        await context.UserFeeds.AddAsync(new UserFeed { UserId = userId, FeedId = feedId }, ct);
    }

    public async Task UnsubscribeUserFromFeedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        var userFeed = await context.UserFeeds
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FeedId == feedId, ct);
        if(userFeed is null)
        {
            throw new KeyNotFoundException($"Relation between user {userId} and {feedId} was not found");
        }
        context.UserFeeds.Remove(userFeed);
    }

    public async Task<bool> UserIsSubscribedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        return await context.UserFeeds.AnyAsync(uf => uf.UserId == userId && uf.FeedId == feedId, ct);
    }
}
