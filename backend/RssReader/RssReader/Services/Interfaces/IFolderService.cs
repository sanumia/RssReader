using RssReader.DTOs.Folder;

namespace RssReader.Services.Interfaces;

public interface IFolderService
{
    Task<ResponseFolderDto> CreateFolderAsync(FolderNameDto folderNameDto, CancellationToken ct = default);
    Task RenameFolderAsync(int folderId, FolderNameDto updateFolderDto, CancellationToken ct = default);
    Task DeleteFolderAsync(int folderId, CancellationToken ct = default);
    Task AddFeedToFolderAsync(int folderId, int feedId, CancellationToken ct = default);
    Task RemoveFeedFromFolderAsync(int folderId, int feedId, CancellationToken ct = default);
    Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(CancellationToken ct = default);
}
