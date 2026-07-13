using RssReader.DTOs.Auth;
using RssReader.Models;
using RssReader.Repositories.Interfaces;
using RssReader.Repositories.UnitOfWork;
using RssReader.Services.Interfaces;

namespace RssReader.Services;

public class AuthService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    TokenService tokenService) : IAuthService
{
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(loginDto.Email, ct);
        if(user is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password or user doesn't exist");
        }

        return new AuthResponseDto
        {
            Token = tokenService.GenerateToken(user),
            UserName = user.UserName,
            Email = user.Email
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
    {
        if (await userRepository.ExistsByEmailAsync(registerDto.Email, ct))
            throw new InvalidOperationException($"User with email {registerDto.Email} already exists");

        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        await userRepository.AddAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return new AuthResponseDto
        {
            Token = tokenService.GenerateToken(user),
            UserName = user.UserName,
            Email = user.Email
        };
    }
}
