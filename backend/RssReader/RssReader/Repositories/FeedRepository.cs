using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FeedRepository(RssReaderDbContext context) : BaseRepository<Feed>(context), IFeedRepository
{
    public async Task<bool> ExistsByUrlAsync(string url)
    {
        return await context.Feeds.AnyAsync(f => f.Url == url);
    }

    public async Task<List<Feed>> GetAllByUserIdAsync(int userId)
    {
        return await context.UserFeeds
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.Feed)
            .ToListAsync();
    }

    public async Task<Feed?> GetByUrlAsync(string url)
    {
        return await context.Feeds.FirstOrDefaultAsync(f => f.Url == url);
    }

    public async Task SubscribeUserToFeedAsync(int userId, int feedId)
    {
        var alreadySubscribed = await UserIsSubscribedAsync(userId, feedId);

        if (alreadySubscribed) 
            return;

        context.UserFeeds.Add(new UserFeed { UserId = userId, FeedId = feedId });
        await context.SaveChangesAsync();
    }

    public async Task UnsubscribeUserFromFeedAsync(int userId, int feedId)
    {
        var userFeed = await context.UserFeeds
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FeedId == feedId);
        if(userFeed is null)
        {
            throw new KeyNotFoundException($"Relation between user {userId} and {feedId} was not found");
        }
        context.UserFeeds.Remove(userFeed);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UserIsSubscribedAsync(int userId, int feedId)
    {
        return await context.UserFeeds.AnyAsync(uf => uf.UserId == userId && uf.FeedId == feedId);
    }
}
