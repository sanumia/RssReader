namespace RssReader.DTOs.User;

public class UserProfileDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
