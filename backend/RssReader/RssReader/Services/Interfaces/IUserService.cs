using RssReader.DTOs.User;

namespace RssReader.Services.Interfaces;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(int userId, CancellationToken ct = default);
    Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken ct = default);
    Task DeleteAccountAsync (int userId, CancellationToken ct = default);
}
