using Azure;
using Microsoft.EntityFrameworkCore;
using RssReader.Constants;
using RssReader.DTOs.FeedItem;
using RssReader.Models;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedItemService(
    FeedItemRepository feedItemRepository, 
    IUserFeedItemRepository userFeedItemRepository, 
    IUnitOfWork unitOfWork,
    CurrentUserService currentUserService) : IFeedItemService
{
    public async Task<List<FeedItemDto>> GetGlobalFeedItemsAsync(
       DateTime? from,
       DateTime? to,
       int pageNumber,
       int pageSize,
       CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int skip = (pageNumber - 1) * pageSize;

        var query = feedItemRepository.GetGlobalFeedItemsQuery(from, to);

        return await ProjectToDto(query)
            .OrderByDescending(dto => dto.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<FeedItemDto>> GetPersonalFeedItemsAsync(
        DateTime? from,
        DateTime? to,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int userId = currentUserService.UserId;
        int skip = (pageNumber - 1) * pageSize;

        var query = feedItemRepository.GetPersonalFeedItemsQuery(userId, from, to);

        return await ProjectToDto(query, userId)
            .OrderByDescending(dto => dto.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<FeedItemDto>> GetPersonalFeedItemsFilteredAsync(
       bool? isRead,
       bool? isFavorite,
       DateTime? from,
       DateTime? to,
       int pageNumber,
       int pageSize,
       CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int userId = currentUserService.UserId;
        int skip = (pageNumber - 1) * pageSize;

        var query = feedItemRepository.GetPersonalFeedItemsQuery(
            userId, isRead, isFavorite, from, to);

        return await ProjectToDto(query, userId)
            .OrderByDescending(dto => dto.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<FeedItemDto>> GetFeedItemsByFeedIdAsync(
       int feedId,
       int pageNumber,
       int pageSize,
       CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int userId = currentUserService.UserId;
        int skip = (pageNumber - 1) * pageSize;

        var query = feedItemRepository.GetByFeedIdQuery(feedId, userId);

        return await ProjectToDto(query, userId)
            .OrderByDescending(dto => dto.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<FeedItemGroupedDto> GetFeedItemsGroupedByFeedIdAsync(
        int feedId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        ValidatePagination(pageNumber, pageSize);

        int userId = currentUserService.UserId;
        int skip = (pageNumber - 1) * pageSize;

        var query = feedItemRepository.GetByFeedIdQuery(feedId, userId);

        var items = await ProjectToDto(query, userId)
            .OrderByDescending(dto => dto.PublishDate)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return GroupByDate(items);
    }

    public async Task<FeedItemDto> GetFeedItemAsync(
        int feedItemId, CancellationToken ct = default)
    {
        int userId = currentUserService.UserId;

        var result = await ProjectToDto(feedItemRepository
            .GetSingleQuery(feedItemId), userId)
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"Feed Item with Id {feedItemId} was not found");

        return result;
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

    private static IQueryable<FeedItemDto> ProjectToDto(IQueryable<FeedItem> query, int? userId = null)
    {
        return query.Select(fi => new FeedItemDto
        {
            Id = fi.Id,
            Title = fi.Title,
            Description = fi.Description,
            Link = fi.Link,
            PublishDate = fi.PublishDate,
            IconUrl = fi.IconUrl,
            Attachments = fi.Attachments,
            IsRead = userId != null && fi.UserFeedItems
                .Any(uf => uf.UserId == userId && uf.IsRead),
            IsFavorite = userId != null && fi.UserFeedItems
                .Any(uf => uf.UserId == userId && uf.IsFavorite)
        });
    }
}
