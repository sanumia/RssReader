using Azure;
using RssReader.DTOs.FeedItem;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedItemService(FeedItemRepository feedItemRepository, IUserFeedItemRepository userFeedItemRepository, IUnitOfWork unitOfWork) : IFeedItemService
{
    private const int DefaultPageSize = 50;
    private const int MaxPageSize = 200;
    public async Task<FeedItemDto> GetFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        var item = await feedItemRepository.GetByIdAsync(feedItemId, ct)
            ?? throw new KeyNotFoundException($"Feed Item with Id {feedItemId} was not found");
        var userState = await userFeedItemRepository.GetAsync(userId, feedItemId, ct);
        
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

    public async Task<List<FeedItemDto>> GetFeedItemFilteredAsync(int userId, FeedItemFilterQuery filter, CancellationToken ct = default)
    {
        var items = await feedItemRepository.GetAllForUserAsync(userId, filter.IsRead, filter.IsFavorite, filter.DateFrom, filter.DateTo, ct);
        var result = new List<FeedItemDto>();
        foreach (var item in items)
        {
            var userState = await userFeedItemRepository.GetAsync(userId, item.Id, ct);
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

    public async Task<FeedItemGroupedDto> GetFeedItemsGroupedAsync(int userId, int feedId, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default)
    {
        if (pageNumber < 1) throw new ArgumentException("Page must be greater than 0.");
        if (pageSize < 1 || pageSize > MaxPageSize)
            throw new ArgumentException("Page size must be between 1 and 200.");

        var skip = (pageNumber - 1) * pageSize;

        var items = await feedItemRepository
            .GetByFeedIdAsync(feedId, skip: skip, take: pageSize, ct);

        var grouped = new FeedItemGroupedDto();
        var today = DateTime.UtcNow.Date;

        foreach (var item in items)
        {
            var userState = await userFeedItemRepository.GetAsync(userId, item.Id, ct);
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
        await userFeedItemRepository.MarkAsReadAsync(userId, feedItemId, isRead);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFeedItemAsync(int userId, int feedItemId, CancellationToken ct = default)
    {
        await userFeedItemRepository.MarkAsRemovedAsync(userId, feedItemId, isRemoved: true, ct);
        await unitOfWork.CommitAsync(ct);
    }
    public async Task ChangeFavoriteStatusAsync(int userId, int feedItemId, bool isFavorite, CancellationToken ct = default)
    {
        await userFeedItemRepository.MarkAsFavoriteAsync(userId, feedItemId, isFavorite);
        await unitOfWork.CommitAsync(ct);
    }
}
