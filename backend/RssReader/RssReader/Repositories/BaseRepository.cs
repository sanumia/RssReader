using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public abstract class BaseRepository<T>(RssReaderDbContext context) : IBaseRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(id, ct);
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);

        return entity;
    }

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct);
        if(entity is null)
        {
            throw new KeyNotFoundException($"{nameof(entity)} with id {id} was not found");
        }
        _dbSet.Remove(entity);
    }
}
