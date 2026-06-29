using RssReader.Models;

namespace RssReader.Repositories.Interfaces;

public interface IFolderRepository : IBaseRepository<Folder> 
{
    Task<List<Folder>> GetAllByUserIdAsync(int userId);
    Task<List<Folder>> GetFoldersWithFeedCountsAsync(int userId);

    Task AddFeedToFolderAsync(int feedId, int folderId);
    Task RemoveFeedFromFolderAsync(int feedId, int folderId);
    Task<List<Feed>> GetFeedsInFolderAsync(int folderId);

}
