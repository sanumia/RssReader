using RssReader.DTOs.Folder;
using RssReader.Models;
using RssReader.Repositories;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class FolderService(IFolderRepository folderRepository, IUnitOfWork unitOfWork) : IFolderService
{
    public async Task AddFeedToFolderAsync(int userId, int folderId, int feedId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id: {folderId} was not found");
        if(folder.UserId == userId)
        {
            await folderRepository.AddFeedToFolderAsync(feedId, folderId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"User with Id{userId}  doesn't have this folder");
        }
    }

    public async Task<ResponseFolderDto> CreateFolderAsync(int userId, FolderNameDto createFolderDto, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(createFolderDto.Name))
            throw new ArgumentException("Folders name can't be empty");
        var existingFolders = await folderRepository.GetAllByUserIdAsync(userId, ct);
        if (existingFolders.Any(f => f.Name.Equals(createFolderDto.Name, StringComparison.OrdinalIgnoreCase)))
            throw new Exception($"Folder with name {createFolderDto.Name} already exists");
        var folder = new Folder { Name = createFolderDto.Name, UserId = userId };
        var created = await folderRepository.AddAsync(folder, ct);
        await unitOfWork.CommitAsync(ct);

        return new ResponseFolderDto
        {
            Id = created.Id,
            Name = created.Name,
            FeedCount = 0
        };
    }

    public async Task DeleteFolderAsync(int userId, int folderId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} not found");

        if(folder.UserId == userId)
        {
            await folderRepository.DeleteAsync(folderId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"User doesn't have this folder");
        }

    }

    public async Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(
    int userId, CancellationToken ct = default)
    {
        var folders = await folderRepository.GetFoldersWithFeedCountsAsync(userId, ct);

        return folders.Select(f => new ResponseFolderDto
        {
            Id = f.Id,
            Name = f.Name,
            FeedCount = f.FeedCount
        }).ToList();
    }

    public async Task RemoveFeedFromFolderAsync(int userId, int folderId, int feedId, CancellationToken ct = default)
    {
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} was not found");

        if(folder.UserId == userId)
        {
            await folderRepository.RemoveFeedFromFolderAsync(feedId, folderId, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"User doesn't have this folder");
        }
    }

    public async Task RenameFolderAsync(int userId, int folderId, FolderNameDto updateFolderDto, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(updateFolderDto.Name))
            throw new ArgumentException("Folder name connot be empty");
        var folder = await folderRepository.GetByIdAsync(folderId, ct)
            ?? throw new KeyNotFoundException($"Folder with id {folderId} was not found");

        if (folder.UserId == userId)
        {
            folder.Name  = updateFolderDto.Name;
            await folderRepository.UpdateAsync(folder, ct);
            await unitOfWork.CommitAsync(ct);
        }
        else
        {
            throw new Exception($"User doesn't have this folder");
        }
    }
}
