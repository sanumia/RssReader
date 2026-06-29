using Microsoft.EntityFrameworkCore;
using RssReader.Data;
using RssReader.Models;
using RssReader.Repositories.Interfaces;

namespace RssReader.Repositories;

public class UserRepository(RssReaderDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
