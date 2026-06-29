using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class BaseRepository<T>(RssReaderDbContext context) : IBaseRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if(entity is null)
        {
            throw new KeyNotFoundException($"{nameof(entity)} with id {id} was not found");
        }
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
