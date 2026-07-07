using RssReader.DTOs.User;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAccountAsync(int userId, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, ct)
            ?? throw new KeyNotFoundException($"User with id {userId} doesn't exist");

        await unitOfWork.Users.DeleteAsync(userId, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<UserProfileDto> GetUserProfileAsync(int userId, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, ct)
            ?? throw new KeyNotFoundException($"User with id {userId} doesn't exist");

        return new UserProfileDto
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }
}
