using RssReader.DTOs.Feed;
using RssReader.Models;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedService(
    IFeedRepository feedRepository,
    IFolderRepository folderRepository,
    IUnitOfWork unitOfWork,
    CurrentUserService currentUserService) : IFeedService
{
    public async Task<List<ResponseFeedDto>> GetAllFeedsAsync(CancellationToken ct = default)
    {
        List<Feed> feeds = await feedRepository.GetAllAsync(ct);

        return feeds.Select(f => new ResponseFeedDto
        {
            Id = f.Id,
            Url = f.Url,
            Title = f.Title,
            IconUrl = f.IconUrl,
        }).ToList();
    }

    public async Task<ResponseFeedDto> CreateFeedAsync(CreateFeedDto createFeedDto, CancellationToken ct = default)
    {
        var feed =  await feedRepository.GetByUrlAsync(createFeedDto.Url, ct);

        if (feed is null)
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
                feed = await feedRepository.AddAsync(feed, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }
            else
            {
                throw new Exception($"Url {createFeedDto.Url} is not valid");
            }
        }

        var isSubscribed = await feedRepository.UserIsSubscribedAsync(currentUserService.UserId, feed.Id, ct);
        if (isSubscribed)
        {
            throw new InvalidOperationException($"You are already subscribed to this feed");
        }
        await feedRepository.SubscribeUserToFeedAsync(currentUserService.UserId, feed.Id, ct);
        await unitOfWork.CommitAsync(ct);

        return new ResponseFeedDto
        {
            Id = feed.Id,
            Url = feed.Url,
            Title = feed.Title,
            IconUrl = feed.IconUrl
        };
    }

    public async Task<List<DashboardFeedDto>> GetFeedsForDashboardAsync(CancellationToken ct = default)
    {
        return await feedRepository.GetDashboardFeedsAsync(currentUserService.UserId, ct);
    }

    public async Task RemoveFeedAsync(int feedId, CancellationToken ct = default)
    {
        var isSubscribe = await feedRepository.UserIsSubscribedAsync(currentUserService.UserId, feedId, ct);
        if (isSubscribe)
        {
            await feedRepository.UnsubscribeUserFromFeedAsync(currentUserService.UserId, feedId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new KeyNotFoundException($"User {currentUserService.UserId} is not subscribe on feed {feedId}");
        }
    }

    public async Task UpdateFeedAsync(int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct = default)
    {
        var feed = await feedRepository.GetByIdAsync(feedId, ct)
            ?? throw new KeyNotFoundException($"Feed with id {feedId} was not found");
        
        if (feed.Url == updateFeedDto.Url) 
            return;

        var isValid = await ValidateFeedAsync(updateFeedDto.Url);
        if(isValid)
        {
            feed.Url = updateFeedDto.Url;
            feed.LastUpdated = DateTime.UtcNow;
            await feedRepository.UpdateAsync(feed, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"Url {updateFeedDto.Url} is not valid");
        }
    }

    private Task<bool> ValidateFeedAsync(string url) => Task.FromResult(true);
}
