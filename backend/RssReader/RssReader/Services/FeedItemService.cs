using Azure;
using Microsoft.EntityFrameworkCore;
using RssReader.Constants;
using RssReader.DTOs.FeedItem;
using RssReader.Models;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

namespace RssReader.Services;

public class FeedItemService(
    FeedItemRepository feedItemRepository,
    IUserFeedItemRepository userFeedItemRepository,
    IUnitOfWork unitOfWork,
    CurrentUserService currentUserService) : IFeedItemService
{
    public async Task<List<FeedItemDto>> GetGlobalFeedItemsAsync(
       GlobalFeedItemsFilter globalFeedItemsFilter,
       CancellationToken ct = default)
    {
        return await GetPagedFeedItemsAsync(
            feedItemRepository.GetGlobalFeedItemsQuery(globalFeedItemsFilter.From, globalFeedItemsFilter.To),
            ToDtoGlobal(),
            globalFeedItemsFilter.PageNumber,
            globalFeedItemsFilter.PageSize,
            ct);
    }

    public async Task<List<FeedItemDto>> GetPersonalFeedItemsAsync(
        GlobalFeedItemsFilter globalFeedItemsFilter,
        CancellationToken ct = default)
    {
        ValidatePagination(globalFeedItemsFilter.PageNumber, globalFeedItemsFilter.PageSize);

        int userId = currentUserService.UserId;
        return await GetPagedFeedItemsAsync(
            feedItemRepository.GetPersonalFeedItemsByDateQuery(userId, globalFeedItemsFilter.From, globalFeedItemsFilter.To),
            ToDtoPersonal(userId),
            globalFeedItemsFilter.PageNumber,
            globalFeedItemsFilter.PageSize,
            ct);
    }

    public async Task<List<FeedItemDto>> GetPersonalFeedItemsFilteredAsync(
       PersonalFeedItemsFilter personalFeedItemsFilter,
       CancellationToken ct = default)
    {
        int userId = currentUserService.UserId;
        return await GetPagedFeedItemsAsync(
           feedItemRepository.GetPersonalFeedItemsQuery(
               userId,
               personalFeedItemsFilter),
           ToDtoPersonal(userId),
           personalFeedItemsFilter.PageNumber,
           personalFeedItemsFilter.PageSize,
           ct);
    }

    public async Task<List<FeedItemDto>> GetFeedItemsByFeedIdAsync(
       int feedId,
       int pageNumber,
       int pageSize,
       CancellationToken ct = default)
    {
        int userId = currentUserService.UserId;
        return await GetPagedFeedItemsAsync(
            feedItemRepository.GetByFeedIdQuery(feedId, userId),
            ToDtoPersonal(userId),
            pageNumber,
            pageSize,
            ct);
    }

    public async Task<FeedItemGroupedDto> GetFeedItemsGroupedByFeedIdAsync(
        int feedId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        int userId = currentUserService.UserId;

        var items = await GetPagedFeedItemsAsync(
            feedItemRepository.GetByFeedIdQuery(feedId, userId),
            ToDtoPersonal(userId),
            pageNumber,
            pageSize,
            ct);

        return GroupByDate(items);
    }

    public async Task<FeedItemDto> GetFeedItemAsync(
        int feedItemId, CancellationToken ct = default)
    {
        int userId = currentUserService.UserId;

        return await feedItemRepository
            .GetSingleQuery(feedItemId)
            .Select(ToDtoPersonal(userId))
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"Feed Item with Id {feedItemId} was not found");
    }

    public async Task MarkAsReadAsync(int feedItemId, bool isRead = true, CancellationToken ct = default)
    {
        await userFeedItemRepository.MarkAsReadAsync(currentUserService.UserId, feedItemId, isRead);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFeedItemAsync(int feedItemId, CancellationToken ct = default)
    {
        await userFeedItemRepository.MarkAsRemovedAsync(currentUserService.UserId, feedItemId, isRemoved: true, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task ChangeFavoriteStatusAsync(
        int feedItemId,
        bool isFavorite,
        CancellationToken ct = default)
    {
        await userFeedItemRepository.MarkAsFavoriteAsync(currentUserService.UserId, feedItemId, isFavorite);
        await unitOfWork.CommitAsync(ct);
    }

    private static void ValidatePagination(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentException(
                "Page number must be greater than 0", nameof(pageNumber));

        if (pageSize < 1 || pageSize > PaginationConstants.MaxPageSize)
            throw new ArgumentException(
                $"Page size must be between 1 and {PaginationConstants.MaxPageSize}",
                nameof(pageSize));
    }

    private static FeedItemGroupedDto GroupByDate(List<FeedItemDto> items)
    {
        var grouped = new FeedItemGroupedDto();
        var today = DateTime.UtcNow.Date;

        foreach (var item in items)
        {
            var daysOld = (today - item.PublishDate.Date).Days;
            if (daysOld <= 0) grouped.Today.Add(item);
            else if (daysOld == 1) grouped.Yesterday.Add(item);
            else if (daysOld <= 7) grouped.LastWeek.Add(item);
            else grouped.Older.Add(item);
        }

        return grouped;
    }

    private async Task<List<FeedItemDto>> GetPagedFeedItemsAsync(
    IQueryable<FeedItem> query,
    Expression<Func<FeedItem, FeedItemDto>> projection,
    int pageNumber,
    int pageSize,
    CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int skip = (pageNumber - 1) * pageSize;

        return await query
            .OrderByDescending(fi => fi.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .Select(projection)
            .ToListAsync(ct);
    }

    private static Expression<Func<FeedItem, FeedItemDto>> ToDtoPersonal(int userId) => fi => new FeedItemDto
    {
        Id = fi.Id,
        Title = fi.Title,
        Description = fi.Description,
        Link = fi.Link,
        PublishDate = fi.PublishDate,
        IconUrl = fi.IconUrl,
        Attachments = fi.Attachments,
        IsRead = fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsRead),
        IsFavorite = fi.UserFeedItems.Any(uf => uf.UserId == userId && uf.IsFavorite)
    };

    private static Expression<Func<FeedItem, FeedItemDto>> ToDtoGlobal() => fi => new FeedItemDto
    {
        Id = fi.Id,
        Title = fi.Title,
        Description = fi.Description,
        Link = fi.Link,
        PublishDate = fi.PublishDate,
        IconUrl = fi.IconUrl,
        Attachments = fi.Attachments,
        IsRead = false,
        IsFavorite = false
    };
}
