using RssReader.DTOs.User;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class UserService(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork,
    CurrentUserService currentUserService) : IUserService
{
    public Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAccountAsync(CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new KeyNotFoundException($"User with id {currentUserService.UserId} doesn't exist");

        await userRepository.DeleteAsync(currentUserService.UserId, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(currentUserService.UserId, ct)
            ?? throw new KeyNotFoundException($"User with id {currentUserService.UserId} doesn't exist");

        return new UserProfileDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        };
    }
}
