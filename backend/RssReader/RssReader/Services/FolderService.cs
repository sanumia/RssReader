using RssReader.DTOs.Folder;
using RssReader.Models;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FolderService(
    IFolderRepository folderRepository, 
    IUnitOfWork unitOfWork,
    CurrentUserService currentUserService) : IFolderService
{
    public async Task AddFeedToFolderAsync(int folderId, int feedId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id: {folderId} was not found");

        await folderRepository.AddFeedToFolderAsync(feedId, folderId, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<ResponseFolderDto> CreateFolderAsync(FolderNameDto folderNameDto, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(folderNameDto.Name))
            throw new ArgumentException("Folders name can't be empty");

        var existingFolders = await folderRepository.GetAllByUserIdAsync(currentUserService.UserId, ct);

        if (existingFolders.Any(f => f.Name.Equals(folderNameDto.Name, StringComparison.OrdinalIgnoreCase)))
            throw new Exception($"Folder with name {folderNameDto.Name} already exists");

        var folder = new Folder { Name = folderNameDto.Name, UserId = currentUserService.UserId };
        var created = await folderRepository.AddAsync(folder, ct);
        await unitOfWork.CommitAsync(ct);

        return new ResponseFolderDto
        {
            Id = created.Id,
            Name = created.Name,
            FeedCount = 0
        };
    }

    public async Task DeleteFolderAsync(int folderId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} not found");

        await folderRepository.DeleteAsync(folderId, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(CancellationToken ct = default)
    {
        var folders = await folderRepository.GetFoldersWithFeedCountsAsync(currentUserService.UserId, ct);

        return folders
            .Select(f => new ResponseFolderDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    FeedCount = f.FeedCount
                })
            .ToList();
    }

    public async Task RemoveFeedFromFolderAsync(int folderId, int feedId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} was not found");

        await folderRepository.RemoveFeedFromFolderAsync(feedId, folderId, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RenameFolderAsync(int folderId, FolderNameDto updateFolderDto, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(updateFolderDto.Name))
            throw new ArgumentException("Folder name connot be empty");
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} was not found");

        folder.Name  = updateFolderDto.Name;
        await folderRepository.UpdateAsync(folder, ct);
        await unitOfWork.CommitAsync(ct);
    }
}
