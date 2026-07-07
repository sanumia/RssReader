using RssReader.Data;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories.UnitOfWork;

public class UnitOfWork(RssReaderDbContext context) : IUnitOfWork, IDisposable
{
    private FeedItemRepository? _feedItems;
    private IFeedRepository? _feeds;
    private IFolderRepository? _folders;
    private IUserRepository? _users;
    private IUserFeedItemRepository? _userFeedItems;
    public IFeedRepository Feeds => _feeds ??= new FeedRepository(context);

    public FeedItemRepository FeedItems => _feedItems ??= new FeedItemRepository(context);

    public IFolderRepository Folders => _folders ??= new FolderRepository(context);

    public IUserFeedItemRepository UserFeedItems => _userFeedItems ??= new UserFeedItemRepository(context);

    public IUserRepository Users => _users ??= new UserRepository(context);

    public async Task<int> CommitAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
