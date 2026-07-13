namespace RssReader.DTOs.User;

public class UserProfileDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
