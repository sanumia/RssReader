using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IFeedRepository Feeds { get; }
    FeedItemRepository FeedItems { get; }
    IFolderRepository Folders { get; }
    IUserFeedItemRepository UserFeedItems { get; }
    IUserRepository Users { get; }

    Task<int> CommitAsync(CancellationToken ct = default);
}
