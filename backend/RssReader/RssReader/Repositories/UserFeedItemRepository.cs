using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class UserFeedItemRepository(RssReaderDbContext context) : IUserFeedItemRepository
{
    public async Task<UserFeedItem?> GetAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        return await context.UserFeedItems
            .FirstOrDefaultAsync(ufi => ufi.UserId == userId && ufi.FeedItemId == feedItemId, ct);
    }

    public async Task<UserFeedItem> CreateAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        var entity = new UserFeedItem
        {
            UserId = userId,
            FeedItemId = feedItemId,
            IsRead = false,
            IsFavorite = false,
            IsRemoved = false
        };

        await context.UserFeedItems.AddAsync(entity, ct);
        return entity;

    }

    public async Task MarkAsFavoriteAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default)
    {
        var userFeedItem = await GetAsync(userId, feedItemId, ct);
        if(userFeedItem is null)
        {
            throw new KeyNotFoundException($"{userId} doesn't have {feedItemId} feedItem");
        }
        userFeedItem.IsFavorite = isFavorite;
    }

    public async Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true, CancellationToken ct = default)
    {
        var userFeedItem = await GetAsync(userId, feedItemId, ct);
        if (userFeedItem is null)
        {
            throw new KeyNotFoundException($"{userId} doesn't have {feedItemId} feedItem");
        }
        userFeedItem.IsRead = isRead;
    }

    public async Task MarkAsRemovedAsync(int userId, int feedItemId, bool isRemoved, CancellationToken ct = default)
    {
        var userFeedItem = await GetAsync(userId, feedItemId, ct);
        if(userFeedItem is null)
        {
            throw new KeyNotFoundException($"{userId} doesn't have {feedItemId} feedItem");
        }
        userFeedItem.IsRemoved = isRemoved;
    }
}
