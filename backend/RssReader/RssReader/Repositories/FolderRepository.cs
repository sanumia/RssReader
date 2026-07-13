using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.DTOs.Folder;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FolderRepository(RssReaderDbContext context) : BaseRepository<Folder>(context), IFolderRepository
{
    public async Task<List<Folder>> GetAllByUserIdAsync(int userId, CancellationToken ct = default)
    {
        return await context.Folders
            .Where(f => f.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<List<ResponseFolderDto>> GetFoldersWithFeedCountsAsync(
    int userId, CancellationToken ct = default)
    {
        return await context.Folders
            .Where(f => f.UserId == userId)
            .Select(f => new ResponseFolderDto
            {
                Id = f.Id,
                Name = f.Name,
                FeedCount = f.FeedFolders.Count
            })
            .ToListAsync(ct);
    }

    public async Task AddFeedToFolderAsync(int feedId, int folderId, CancellationToken ct = default)
    {
        var alreadyAdded = await context.FeedFolders
            .AnyAsync(ff => ff.FeedId == feedId && ff.FolderId == folderId, ct);
        if (alreadyAdded) return;
        await context.FeedFolders.AddAsync(new FeedFolder { FeedId = feedId, FolderId = folderId }, ct);
    }

    public async Task RemoveFeedFromFolderAsync(int feedId, int folderId, CancellationToken ct = default)
    {
        var feedFolder = await context.FeedFolders
            .FirstOrDefaultAsync(ff => ff.FeedId == feedId && ff.FolderId == folderId, ct);
        if(feedFolder is null)
        {
            throw new KeyNotFoundException($"Relation between feed {feedId} and {folderId} was not found");
        }
        context.FeedFolders.Remove(feedFolder);
    }

    public async Task<List<Feed>> GetFeedsInFolderAsync(int folderId, CancellationToken ct = default)
    {
        return await context.FeedFolders
            .Where(ff => ff.FolderId == folderId)
            .Select(ff => ff.Feed)
            .ToListAsync(ct);
    }

    public async Task<List<ResponseFolderDto>> GetFoldersByFeedIdAsync(int feedId, CancellationToken ct = default)
    {
        return await context.FeedFolders
            .Where(ff => ff.FeedId == feedId)
            .Select(ff => new ResponseFolderDto
            {
                Id = ff.Folder.Id,
                Name = ff.Folder.Name,
                FeedCount = ff.Folder.FeedFolders.Count
            })
            .ToListAsync(ct);
    }
}
