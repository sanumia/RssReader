namespace RssReader.Models;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<FeedFolder> FeedFolders { get; set; } = new List<FeedFolder>();

}
