using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<int> CommitAsync(CancellationToken ct = default);
}
