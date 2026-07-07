using RssReader.DTOs.FeedItem;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedItemService(IUnitOfWork unitOfWork) : IFeedItemService
{
    public async Task<FeedItemDto> GetFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        var item = await unitOfWork.FeedItems.GetByIdAsync(feedItemId, ct)
            ?? throw new KeyNotFoundException($"Feed Item with Id {feedItemId} wa not found");
        var userState = await unitOfWork.UserFeedItems.GetAsync(userId, feedItemId, ct);
        
        return new FeedItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Link = item.Link,
            PublishDate = item.PublishDate,
            IconUrl = item.IconUrl,
            Attachments = item.Attachments,
            IsRead = userState?.IsRead ?? false,
            IsFavorite = userState?.IsFavorite ?? false,
        };
    }

    public async Task<List<FeedItemDto>> GetFeedItemFilteredAsync(int userId, int feedId, FeedItemFilterQuery filter, CancellationToken ct = default)
    {
        var items = await unitOfWork.FeedItems.GetAllForUserAsync(userId, filter.IsRead, filter.IsFavorite, filter.DateFrom, filter.DateTo, ct);
        var result = new List<FeedItemDto>();
        foreach (var item in items)
        {
            var userState = await unitOfWork.UserFeedItems.GetAsync(userId, item.Id, ct);
            result.Add(new FeedItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Link = item.Link,
                PublishDate = item.PublishDate,
                IconUrl = item.IconUrl,
                Attachments = item.Attachments,
                IsRead = userState?.IsRead ?? false,
                IsFavorite = userState?.IsFavorite ?? false,
            });
        }

        return result;
    }

    public async Task<FeedItemGroupedDto> GetFeedItemsGroupedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        var items = await unitOfWork.FeedItems.GetByFeedIdAsync(feedId, skip: 0, take: 100, ct);
        var grouped = new FeedItemGroupedDto();
        var today = DateTime.UtcNow.Date;

        foreach (var item in items)
        {
            var userState = await unitOfWork.UserFeedItems.GetAsync(userId, item.Id, ct);
            if (userState?.IsRemoved == true) continue;

            var itemDto = new FeedItemDto()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IconUrl = item.IconUrl,
                Link = item.Link,
                Attachments = item.Attachments,
                PublishDate = item.PublishDate,
                IsRead = userState?.IsRead ?? false,
                IsFavorite = userState?.IsFavorite ?? false
            };

            var daysOld = (today - item.PublishDate.Date).Days;
            if (daysOld <= 0) grouped.Today.Add(itemDto);
            else if (daysOld == 1) grouped.Yesterday.Add(itemDto);
            else if (daysOld <= 7) grouped.LastWeek.Add(itemDto);
            else grouped.Older.Add(itemDto);
        }

        return grouped;
    }

    public async Task MarkAsReadAsync(int userId, int feedItemId, bool isRead = true, CancellationToken ct = default)
    {
        await unitOfWork.UserFeedItems.MarkAsReadAsync(userId, feedItemId, isRead);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        await unitOfWork.UserFeedItems.MarkAsRemovedAsync(userId, feedItemId, IsRemoved: true, ct);
        await unitOfWork.CommitAsync(ct);
    }
    public async Task ChangeFavoriteStatusAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default)
    {
        await unitOfWork.UserFeedItems.MarkAsFavoriteAsync(userId, feedItemId, isFavorite);
        await unitOfWork.CommitAsync(ct);
    }
}
