using RssReader.DTOs.Folder;
using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IFolderRepository : IBaseRepository<Folder> 
{
    Task<List<Folder>> GetAllByUserIdAsync(int userId, CancellationToken ct = default);
    Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(int userId, CancellationToken ct = default);

    Task AddFeedToFolderAsync(int feedId, int folderId, CancellationToken ct = default);
    Task RemoveFeedFromFolderAsync(int feedId, int folderId, CancellationToken ct = default);
    Task<List<Feed>> GetFeedsInFolderAsync(int folderId, CancellationToken ct = default);
    Task<List<ResponseFolderDto>> GetFoldersByFeedIdAsync(int feedId, CancellationToken ct = default);
}
