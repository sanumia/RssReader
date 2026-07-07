using RssReader.DTOs.Feed;

namespace RssReader.Services.Interfaces;

public interface IFeedService
{
    Task<ResponseFeedDto> CreateFeedAsync(int userId, CreateFeedDto createFeedDto, CancellationToken ct = default);
    Task UpdateFeedAsync(int userId, int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct = default);
    Task RemoveFeedAsync(int userId, int feedId, CancellationToken ct = default);
    Task<List<DashboardFeedDto>> GetFeedsForDashboardAsync(int userId, CancellationToken ct = default);
}
