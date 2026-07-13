using RssReader.Data;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories.UnitOfWork;

public class UnitOfWork(RssReaderDbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);
}
