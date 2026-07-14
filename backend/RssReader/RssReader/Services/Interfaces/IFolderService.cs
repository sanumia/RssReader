using RssReader.DTOs.Folder;

namespace RssReader.Services.Interfaces;

public interface IFolderService
{
    Task<ResponseFolderDto> CreateFolderAsync(int userId, FolderNameDto folderNameDto, CancellationToken ct = default);
    Task RenameFolderAsync(int userId, int folderId, FolderNameDto updateFolderDto, CancellationToken ct = default);
    Task DeleteFolderAsync(int userId, int folderId, CancellationToken ct = default);
    Task AddFeedToFolderAsync(int userId, int folderId, int feedId, CancellationToken ct = default);
    Task RemoveFeedFromFolderAsync(int userId, int folderId, int feedId, CancellationToken ct = default);
    Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(int userId, CancellationToken ct = default);
}
