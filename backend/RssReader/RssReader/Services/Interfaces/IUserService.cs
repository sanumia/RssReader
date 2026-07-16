using RssReader.DTOs.User;

namespace RssReader.Services.Interfaces;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(CancellationToken ct = default);
    Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken ct = default);
    Task DeleteAccountAsync (CancellationToken ct = default);
}
