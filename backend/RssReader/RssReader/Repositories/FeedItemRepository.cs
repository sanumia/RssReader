using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.DTOs.FeedItem;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FeedItemRepository(RssReaderDbContext context) : BaseRepository<FeedItem>(context)
{
    public IQueryable<FeedItem> GetGlobalFeedItemsQuery(DateTime? from, DateTime? to)
    {
        var query = context.FeedItems.AsQueryable();

        if (from.HasValue)
            query = query.Where(fi => fi.PublishDate >= from.Value);
        if (to.HasValue)
            query = query.Where(fi => fi.PublishDate <= to.Value);

        return query;
    }

    public IQueryable<FeedItem> GetPersonalFeedItemsQuery(int userId, DateTime? from, DateTime? to)
    {
        var query = context.FeedItems
            .Where(fi => context.UserFeeds.Any(uf => uf.UserId == userId && uf.FeedId == fi.FeedId))
            .Where(fi => !fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsRemoved));

        if (from.HasValue)
            query = query.Where(fi => fi.PublishDate >= from.Value);
        if (to.HasValue)
            query = query.Where(fi => fi.PublishDate <= to.Value);

        return query;
    }

    public IQueryable<FeedItem> GetPersonalFeedItemsQuery(
        int userId,
        bool? isRead,
        bool? isFavorite,
        DateTime? from,
        DateTime? to)
    {
        var query = context.FeedItems
            .Where(fi => context.UserFeeds.Any(uf => uf.UserId == userId && uf.FeedId == fi.FeedId))
            .Where(fi => !fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsRemoved));

        if (isRead.HasValue)
            query = query.Where(fi => fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsRead == isRead.Value));
        if (isFavorite.HasValue)
            query = query.Where(fi => fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsFavorite == isFavorite.Value));

        if (from.HasValue)
            query = query.Where(fi => fi.PublishDate >= from.Value);
        if (to.HasValue)
            query = query.Where(fi => fi.PublishDate <= to.Value);

        return query;
    }

    public IQueryable<FeedItem> GetByFeedIdQuery(int feedId, int userId)
    {
        return context.FeedItems
            .Where(fi => fi.FeedId == feedId)
            .Where(fi => context.UserFeeds
                .Any(uf => uf.UserId == userId && uf.FeedId == fi.FeedId))
            .Where(fi => !fi.UserFeedItems
                .Any(uf => uf.UserId == userId && uf.IsRemoved));
    }

    public IQueryable<FeedItem> GetSingleQuery(int feedItemId)
    {
        return context.FeedItems.Where(fi => fi.Id == feedItemId);
    }

    public async Task AddRangeAsync(
       List<FeedItem> feedItems, CancellationToken ct = default)
    {
        await context.FeedItems.AddRangeAsync(feedItems, ct);
    }
}
