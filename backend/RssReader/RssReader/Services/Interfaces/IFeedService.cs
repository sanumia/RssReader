using RssReader.DTOs.Feed;

namespace RssReader.Services.Interfaces;

public interface IFeedService
{
    Task<List<ResponseFeedDto>> GetAllFeedsAsync(CancellationToken ct = default);
    Task<ResponseFeedDto> CreateFeedAsync(CreateFeedDto createFeedDto, CancellationToken ct = default);
    Task UpdateFeedAsync(int feedId, UpdateFeedDto updateFeedDto, CancellationToken ct = default);
    Task RemoveFeedAsync(int feedId, CancellationToken ct = default);
    Task<List<DashboardFeedDto>> GetFeedsForDashboardAsync(CancellationToken ct = default);
}
