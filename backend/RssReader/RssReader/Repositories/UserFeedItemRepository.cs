using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class UserFeedItemRepository(RssReaderDbContext context) : IUserFeedItemRepository
{
    public async Task<UserFeedItem?> GetAsync(int userId, int feedItemId)
    {
        return await context.UserFeedItems
            .FirstOrDefaultAsync(ufi => ufi.UserId == userId && ufi.FeedItemId == feedItemId);
    }

    public async Task<UserFeedItem> CreateAsync(int userId, int feedItemId)
    {
        var entity = new UserFeedItem
        {
            UserId = userId,
            FeedItemId = feedItemId,
            IsRead = false,
            IsFavorite = false
        };

        context.UserFeedItems.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task MarkAsFavoriteAsync(int userId, int feedItemId, bool isFavorite)
    {
        var feed = await GetAsync(userId, feedItemId);
        if(feed is null)
        {
            throw new KeyNotFoundException($"{userId} doesn't have {feedItemId} feedItem");
        }
        feed.IsFavorite = isFavorite;
        await context.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true)
    {
        var feed = await GetAsync(userId, feedItemId);
        if (feed is null)
        {
            throw new KeyNotFoundException($"{userId} doesn't have {feedItemId} feedItem");
        }
        feed.IsRead = isRead;
        await context.SaveChangesAsync();
    }
}
