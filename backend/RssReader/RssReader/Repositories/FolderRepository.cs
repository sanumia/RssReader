using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class FolderRepository(RssReaderDbContext context) : BaseRepository<Folder>(context), IFolderRepository
{
    public async Task<List<Folder>> GetAllByUserIdAsync(int userId)
    {
        return await context.Folders
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Folder>> GetFoldersWithFeedCountsAsync(int userId)
    {
        return await context.Folders
            .Where(f => f.UserId == userId)
            .Include(f => f.FeedFolders)
            .ToListAsync();
    }

    public async Task AddFeedToFolderAsync(int feedId, int folderId)
    {
        var alreadyAdded = await context.FeedFolders
            .AnyAsync(ff => ff.FeedId == feedId && ff.FolderId == folderId);
        if (alreadyAdded) return;
        context.FeedFolders.Add(new FeedFolder { FeedId = feedId, FolderId = folderId });
        await context.SaveChangesAsync();
    }

    public async Task RemoveFeedFromFolderAsync(int feedId, int folderId)
    {
        var feedFolder = await context.FeedFolders
            .FirstOrDefaultAsync(ff => ff.FeedId == feedId && ff.FolderId == folderId);
        if(feedFolder is null)
        {
            throw new KeyNotFoundException($"Relation between feed {feedId} and {folderId} was not found");
        }
        context.FeedFolders.Remove(feedFolder);
        await context.SaveChangesAsync();
    }

    public async Task<List<Feed>> GetFeedsInFolderAsync(int folderId)
    {
        return await context.FeedFolders
            .Where(ff => ff.FolderId == folderId)
            .Select(ff => ff.Feed)
            .ToListAsync();
    }
}
