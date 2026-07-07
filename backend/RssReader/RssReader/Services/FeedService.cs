using RssReader.DTOs.Feed;
using RssReader.Models;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedService(IUnitOfWork unitOfWork) : IFeedService
{
    public async Task<ResponseFeedDto> CreateFeedAsync(int userId, CreateFeedDto createFeedDto, CancellationToken ct = default)
    {
        var existingFeed =  await unitOfWork.Feeds.GetByUrlAsync(createFeedDto.Url, ct);
        Feed feed;
        if(existingFeed is null)
        {
            var isValid = await ValidateFeedAsync(createFeedDto.Url);
            if (isValid)
            {
                feed = new Feed
                {
                    Url = createFeedDto.Url,
                    IsActive = true,
                    LastUpdated = DateTime.UtcNow,
                };
                feed = await unitOfWork.Feeds.AddAsync(feed, ct);
                await unitOfWork.CommitAsync(ct);
            }
            else
            {
                throw new Exception($"Url {createFeedDto.Url} is not valid");
            }
        }
        else
        {
            feed = existingFeed;
        }

        await unitOfWork.Feeds.SubscribeUserToFeedAsync(userId, feed.Id, ct);
        await unitOfWork.CommitAsync(ct);

        return new ResponseFeedDto
        {
            Id = feed.Id,
            Url = feed.Url,
            Title = feed.Title,
            IconUrl = feed.IconUrl
        };
    }

    public async Task<List<DashboardFeedDto>> GetFeedsForDashboardAsync(int userId, CancellationToken ct = default)
    {
        var feeds = await unitOfWork.Feeds.GetAllByUserIdAsync(userId, ct);
        var folders = await unitOfWork.Folders.GetFoldersWithFeedCountsAsync(userId, ct);

        return feeds.Select(feed => new DashboardFeedDto
        {
            Id = feed.Id,
            Url = feed.Url,
            Title = feed.Title,
            IconUrl = feed.IconUrl,
            FolderNames = folders
                .Where(f => f.FeedFolders.Any(ff => ff.FeedId == feed.Id))
                .Select(f => f.Name)
                .ToList(),
            FeedItemCount = feed.FeedItems?.Count ?? 0
        }).ToList();
    }

    public async Task RemoveFeedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        var isSubscribe = await unitOfWork.Feeds.UserIsSubscribedAsync(userId, feedId, ct);
        if (isSubscribe)
        {
            await unitOfWork.Feeds.UnsubscribeUserFromFeedAsync(userId, feedId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new KeyNotFoundException($"User {userId} is not subscribe on feed {feedId}");
        }
    }

    public async Task UpdateFeedAsync(int userId, int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct = default)
    {
        var feed = await unitOfWork.Feeds.GetByIdAsync(feedId, ct)
            ?? throw new KeyNotFoundException($"Feed with id {feedId} was not found");
        
        if (feed.Url == updateFeedDto.Url) 
            return;

        var isValid = await ValidateFeedAsync(updateFeedDto.Url);
        if(isValid)
        {
            feed.Url = updateFeedDto.Url;
            feed.LastUpdated = DateTime.UtcNow;
            await unitOfWork.Feeds.UpdateAsync(feed, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"Url {updateFeedDto.Url} is not valid");
        }
    }

    private Task<bool> ValidateFeedAsync(string url) => Task.FromResult(true);
}
