using RssReader.DTOs.Auth;

namespace RssReader.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
}
