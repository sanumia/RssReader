using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FeedItemRepository(RssReaderDbContext context) : BaseRepository<FeedItem>(context)
{
    public async Task<List<FeedItem>> GetAllAsync(DateTime? from, DateTime? to, int skip, int take, CancellationToken ct = default)
    {
        var query = context.FeedItems.AsQueryable();

        if(from.HasValue)
            query = query.Where(fi => fi.PublishDate >=  from.Value);

        if (to.HasValue)
            query = query.Where(fi => fi.PublishDate <= to.Value);

        return await query
            .OrderByDescending(fi => fi.PublishDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }
    public async Task AddRangeAsync(List<FeedItem> feedItems, CancellationToken ct = default)
    {
        await context.FeedItems.AddRangeAsync(feedItems, ct);
    }

    public async Task<List<FeedItem>> GetAllForUserAsync
        (int userId,
        bool? isRead, 
        bool? isFavorite, 
        DateTime? from, 
        DateTime? to, 
        CancellationToken ct = default)
    {
        var query = context.UserFeedItems
            .Where(ufi => ufi.UserId == userId && !ufi.IsRemoved)
            .AsQueryable();

        if (isRead.HasValue)
            query = query.Where(ufi => ufi.IsRead == isRead.Value);

        if (isFavorite.HasValue)
            query = query.Where(ufi => ufi.IsFavorite == isFavorite.Value);

        if (from.HasValue)
            query = query.Where(ufi => ufi.FeedItem.PublishDate >= from.Value);

        if (to.HasValue)
            query = query.Where(ufi => ufi.FeedItem.PublishDate <= to.Value);

        return await query
            .OrderByDescending(ufi => ufi.FeedItem.PublishDate)
            .Select(ufi => ufi.FeedItem)
            .ToListAsync(ct);
    }

    public async Task<List<FeedItem>> GetByFeedIdAsync
        (int feedId, 
        int skip, 
        int take, 
        CancellationToken ct = default)
    {
        return await context.FeedItems
            .Where(fi => fi.FeedId == feedId)
            .OrderByDescending(fi => fi.PublishDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }
}
