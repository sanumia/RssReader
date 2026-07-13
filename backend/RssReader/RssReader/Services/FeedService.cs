using RssReader.DTOs.Feed;
using RssReader.Models;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FeedService(
    IFeedRepository feedRepository,
    IFolderRepository folderRepository,
    IUnitOfWork unitOfWork) : IFeedService
{
    public async Task<ResponseFeedDto> CreateFeedAsync(int userId, CreateFeedDto createFeedDto, CancellationToken ct = default)
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

        var isSubscribed = await feedRepository.UserIsSubscribedAsync(userId, feed.Id, ct);
        if (isSubscribed)
        {
            throw new InvalidOperationException($"You are already subscribed to this feed");
        }
        await feedRepository.SubscribeUserToFeedAsync(userId, feed.Id, ct);
        await unitOfWork.CommitAsync(ct);

        return new ResponseFeedDto
        {
            Id = feed.Id,
            Url = feed.Url,
            Title = feed.Title,
            IconUrl = feed.IconUrl
        };
    }

    public async Task<List<DashboardFeedDto>> GetFeedsForDashboardAsync(
    int userId, CancellationToken ct = default)
    {
        return await feedRepository.GetDashboardFeedsAsync(userId, ct);
    }

    public async Task RemoveFeedAsync(int userId, int feedId, CancellationToken ct = default)
    {
        var isSubscribe = await feedRepository.UserIsSubscribedAsync(userId, feedId, ct);
        if (isSubscribe)
        {
            await feedRepository.UnsubscribeUserFromFeedAsync(userId, feedId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new KeyNotFoundException($"User {userId} is not subscribe on feed {feedId}");
        }
    }

    public async Task UpdateFeedAsync(int userId, int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct = default)
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
