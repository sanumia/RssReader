namespace RssReader.Models;

public class FeedFolder
{
    public int FeedId { get; set; }
    public Feed Feed { get; set; } = null!;
    public int FolderId { get; set; }
    public Folder Folder { get; set; } = null!;
}
